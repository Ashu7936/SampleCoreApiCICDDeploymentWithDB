using Microsoft.EntityFrameworkCore;
using SampleCoreApiCICDDeploymentWithDB.Models;

namespace SampleCoreApiCICDDeploymentWithDB.DataAccess
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
