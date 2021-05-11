using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Baron.Entity
{
    public class BContext : IdentityDbContext<BUser, IdentityRole<Guid>, Guid>
    {
        public BContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<BMonitor> Monitors { get; set; }
        public DbSet<BMonitorStep> MonitorSteps { get; set; }
        public DbSet<BMonitorStepLog> MonitorStepLogs { get; set; }
    }
}