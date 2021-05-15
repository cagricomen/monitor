using System;
using System.Linq;
using System.Threading.Tasks;
using Baron.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Baron.Web.Controllers
{
    [Authorize]
    public class MonitoringController : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute]Guid? id)
        {
            if (id.HasValue)
            {
                if (id.Value == Guid.Empty)
                {
                    return Error("You must send monitor id to get.");
                }
                var monitor = await Db.Monitors.FirstOrDefaultAsync(x => x.MonitorId == id.Value && x.UserId == UserId);
                if (monitor == null)
                {
                    return Error("Monitor not found", code: 404);
                }

                var url = string.Empty;
                var monitorStepRequest = await Db.MonitorSteps.FirstOrDefaultAsync(x => x.MonitorId == monitor.MonitorId && x.Type == BMonitorStepTypes.Request);
                if (monitorStepRequest != null)
                {
                    var requestSettings = monitorStepRequest.SettingsAsRequest();
                    if (requestSettings != null)
                    {
                        url = requestSettings.Url;
                    }
                }
                return Success(data: new
                {
                    monitor.MonitorId,
                    monitor.CreatedDate,
                    monitor.LastCheckDate,
                    monitor.MonitorStatus,
                    monitor.Name,
                    monitor.TestStatus,
                    monitor.UpTime,
                    monitor.UpdatedDate,
                    Url = url
                });
            }
            var list = await Db.Monitors.Where(x => x.UserId == UserId).ToListAsync();
            return Success(null, list);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BMMonitorSave value)
        {
            var userId = UserManager.GetUserId(User);
            if (string.IsNullOrEmpty(value.Name))
            {
                return Error("Name is required.");
            }
            var monitorCheck = await Db.Monitors.AnyAsync(x=>x.Name.Equals(value.Name) && x.UserId == UserId);
            if(monitorCheck)
            {
                return Error("project name is already.choose different name");
            }
            BMonitor data = null;
            if (value.Id != Guid.Empty)
            {
                data = await Db.Monitors.FirstOrDefaultAsync(x => x.MonitorId == value.Id && x.UserId == UserId);
                if (data == null) return Error("Monitor not found.");

                data.UpdatedDate = DateTime.UtcNow;
                data.Name = value.Name;
            }
            else
            {
                data = new BMonitor
                {
                    MonitorId = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    Name = value.Name,
                    UserId = UserId
                };
            }
            Db.Monitors.Add(data);

            var monitorStepData = new BDSMonitorStepSettingsRequest
            {
                Url = value.Url
            };

            var step = await Db.MonitorSteps.FirstOrDefaultAsync(x => x.MonitorId == data.MonitorId && x.Type == BMonitorStepTypes.Request);
            if (step != null)
            {
                var requestSettings = step.SettingsAsRequest() ?? new BDSMonitorStepSettingsRequest();
                requestSettings.Url = value.Url;
                step.Settings = JsonConvert.SerializeObject(requestSettings);
            }
            else
            {
                step = new BMonitorStep
                {
                    MonitorStepId = Guid.NewGuid(),
                    Type = BMonitorStepTypes.Request,
                    MonitorId = data.MonitorId,
                    Settings = JsonConvert.SerializeObject(monitorStepData)
                };
                Db.MonitorSteps.Add(step);
            }
            var result = await Db.SaveChangesAsync();
            if (result > 0)
                return Success("Monitoring saved successfully.", new
                {
                    Id = data.MonitorId
                });
            else
                return Error("Something wrong.");
        }
    }
    public class MonitoringModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
