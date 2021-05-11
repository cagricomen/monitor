using System;
using System.Linq;
using System.Threading.Tasks;
using Baron.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Baron.Web.Controllers
{
    [Authorize]
    public class MonitoringController : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await Db.Monitors.ToListAsync();
            return Success(null, list);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BMonitor value)
        {
            if (string.IsNullOrEmpty(value.Name))
            {
                return Error("Name is required.");
            }
            var dataObject = new BMonitor
            {
                CreatedDate = DateTime.UtcNow,
                Name = value.Name
            };


            Db.Monitors.Add(value);
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
