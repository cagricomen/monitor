using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baron.Entity
{

    [Table("Monitor")]
    public class BMonitor
    {
        [Key]
        public Guid MonitorId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Name { get; set; }
        public BMonitorStatusTypes MonitorStatus { get; set; }
        public BTestStatusTypes TestStatus { get; set; }
        public DateTime LastCheckDate { get; set; }
        public decimal UpTime { get; set; }
        public int LoadTime { get; set; }
        public int Monitortime { get; set; }
    }
    public enum BMonitorStatusTypes : short
    {
        Unknown = 0,
        Up = 1,
        Down = 2,
        Warning = 3
    }
    public enum BTestStatusTypes : short
    {
        Unknown = 0,
        AllPassed = 1,
        Fail = 2,
        Warning = 3
    }
}
