using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labmin.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Labmin.Api.Data
{
    public class LabminDbContext : DbContext
    {
        public LabminDbContext(DbContextOptions<LabminDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pool> Pools { get; set; }
    }
}
