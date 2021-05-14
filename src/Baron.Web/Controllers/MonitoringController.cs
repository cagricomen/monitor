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
        public async Task<IActionResult> Get( Guid? id)
        {
            if(id.HasValue)
            {
                if(id.Value == Guid.Empty)
                {
                    return Error("You must send monitor id to get.");
                }
                var monitor = await Db.Monitors.FirstOrDefaultAsync(x=>x.MonitorId == id.Value && x.UserId == UserId);
                if(monitor == null)
                {
                    return Error("Monitor not found",code:404);
                }
                return Success(data: new {
                     monitor.MonitorId,
                    monitor.CreatedDate,
                    monitor.LastCheckDate,
                    monitor.MonitorStatus,
                    monitor.Name,
                    monitor.TestStatus,
                    monitor.UpTime,
                    monitor.UpdatedDate
                });
            }
            var list = await Db.Monitors.ToListAsync();
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
            var dataObject = new BMonitor
            {
                MonitorId = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                Name = value.Name,
                UserId = UserId
            };


            Db.Monitors.Add(dataObject);

            var monitorStepData = new BDSMonitorStepSettingsRequest
            {
                Url = value.Url
            };

            var monitorStep = new BMonitorStep
            {
                MonitorStepId = Guid.NewGuid(),
                Type = BMonitorStepTypes.Request,
                MonitorId = dataObject.MonitorId,
                Settings = JsonConvert.SerializeObject(monitorStepData)
            };
            Db.MonitorSteps.Add(monitorStep);
            var result = await Db.SaveChangesAsync();
            if (result > 0)
                return Success("Monitoring saved successfully.", new
                {
                    Id = dataObject.MonitorId
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
