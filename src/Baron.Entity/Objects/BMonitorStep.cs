using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Baron.Entity
{
    [Table("MonitorStep")]
    public class BMonitorStep
    {
        [Key]
        public Guid MonitorStepId { get; set; }
        public Guid MonitorId { get; set; }
        public BMonitorStepTypes Type { get; set; }
        public string Settings { get; set; }

        public BDSMonitorStepSettingsRequest SettingsAsRequest()
        {
            return JsonConvert.DeserializeObject<BDSMonitorStepSettingsRequest>(Settings);
        }
    }
    public enum BMonitorStepTypes : short
    {
        Request = 1,
        StatusCode = 2,
        HeaderExists = 3,
        BodyContains = 4,
    }
    public class BDSMonitorStepSettingsRequest
    {
        public string Url { get; set; }
    }


}