using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioInsightsPortal.Data.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BioInsightsDbContext>
    {
        public BioInsightsDbContext CreateDbContext(string[] args)
        {
            // 🔹 Make sure it points to the Api project folder where appsettings.json lives
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../BioInsightsPortal.Api");

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<BioInsightsDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new BioInsightsDbContext(optionsBuilder.Options);
        }
    }
}
