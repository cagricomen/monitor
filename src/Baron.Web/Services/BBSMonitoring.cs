using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Baron.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Baron.Web
{
    public class BBSMonitoring : IHostedService, IDisposable
    {
        public IServiceProvider Services { get; }
        private CancellationToken _token;

        public BBSMonitoring(IServiceProvider services)
        {
            Services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _token = cancellationToken;
            DoWork();
            return Task.CompletedTask;
        }

        private async void DoWork()
        {
            while (true)
            {
                using (var scope = Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<BContext>();
                    var steps = await db.MonitorSteps
                                    .Where(x => x.Type == BMonitorStepTypes.Request && x.Status != BMonitorStepStatusTypes.Processing && x.LastCheckDate.AddSeconds(x.Interval) > DateTime.UtcNow)
                                    .OrderBy(x => x.LastCheckDate)
                                    .Take(20)
                                    .ToListAsync();

                    foreach (var step in steps)
                    {
                        var settings = step.SettingsAsRequest();
                        if (!string.IsNullOrEmpty(settings.Url))
                        {
                            var log = new BMonitorStepLog
                            {
                                MonitorId = step.MonitorId,
                                MonitorStepId = step.MonitorStepId,
                                StartDate = DateTime.UtcNow,
                                Interval = step.Interval,
                                Status = BMonitorStepStatusTypes.Processing
                            };
                            db.Add(log);
                            await db.SaveChangesAsync(_token);

                            try
                            {
                                var client = new HttpClient();
                                client.Timeout = TimeSpan.FromSeconds(15);
                                var result = await client.GetAsync(settings.Url, _token);
                                if (result.IsSuccessStatusCode)
                                {
                                    log.Status = BMonitorStepStatusTypes.Success;
                                }
                                else
                                {
                                    log.Status = BMonitorStepStatusTypes.Fail;
                                }
                            }
                            catch (HttpRequestException rex)
                            {
                                log.Log = rex.Message;
                                log.Status = BMonitorStepStatusTypes.Fail;
                            }
                            catch (Exception ex)
                            {
                                log.Log = ex.Message;
                                log.Status = BMonitorStepStatusTypes.Error;
                            }
                            finally
                            {
                                log.EndDate = DateTime.UtcNow;
                            }
                            if (log.Status == BMonitorStepStatusTypes.Success)
                                step.Status = BMonitorStepStatusTypes.Success;
                            else if (log.Status == BMonitorStepStatusTypes.Error)
                                step.Status = BMonitorStepStatusTypes.Error;
                            else
                                step.Status = BMonitorStepStatusTypes.Fail;
                        }
                        step.LastCheckDate = DateTime.UtcNow;
                        await db.SaveChangesAsync(_token);
                    }
                }
                await Task.Delay(500, _token);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public void Dispose()
        {
        }

    }
}
