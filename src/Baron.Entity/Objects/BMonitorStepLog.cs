using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baron.Entity
{
    [Table("MonitorStepLog")]
    public class BMonitorStepLog
    {
        [Key]
        public Guid MonitorStepLogId { get; set; }
        public Guid MonitorStepId { get; set; }
        public Guid MonitorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BMonitorStepStatusTypes Status { get; set; }
        public string Log { get; set; }
    }
    public enum BMonitorStepStatusTypes : short
    {
        Fail = 0,
        Success = 1
    }
}