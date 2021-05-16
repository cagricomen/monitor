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
        public int Interval { get; set; }
        public BMonitorStepStatusTypes Status { get; set; }
        public DateTime LastCheckDate { get; set; }
        public BDSMonitorStepSettingsRequest SettingsAsRequest()
        {
            return JsonConvert.DeserializeObject<BDSMonitorStepSettingsRequest>(Settings);
        }
    }
    public enum BMonitorStepStatusTypes : short
    {
        Unknown = 0,
        Pending = 1,
        Processing = 2,
        Success = 3,
        Warning = 4,
        Fail = 5,
        Error = 6
    }
    public enum BMonitorStepTypes : short
    {
        Unknown = 0,
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