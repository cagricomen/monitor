using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Baron.Entity
{
    public class BContext : IdentityDbContext<BUser>
    {
        public BContext(DbContextOptions options) : base(options)
        {
        }
    }
}