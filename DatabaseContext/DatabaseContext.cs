using Microsoft.EntityFrameworkCore;
using ServiceRequests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceRequests.Service
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ServiceModel> Services { get; set; }

        #region SQLite
        /*To use SQLite*/
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=test.db", options =>
            {
                options.MigrationsAssembly(System.Reflection.Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }
        #endregion
    }
}
