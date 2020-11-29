using Microsoft.EntityFrameworkCore;
using NovibetIPStackAPI.Core.Models.BatchRelated;
using NovibetIPStackAPI.Core.Models.IPRelated;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace NovibetIPStackAPI.Infrastructure.Persistence
{
    /// <summary>
    /// The application's database context which will inherit from the DbContext.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { 
        }

        /// <summary>
        /// Contains the IP Geolocation details of an IP address.
        /// </summary>
        public DbSet<IPDetailsModel> IPDetails { get; set; }
        /// <summary>
        /// Contains useful information regarding the batch update jobs that are processed via tasks.
        /// </summary>
        public DbSet<JobModel> Jobs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return result;
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
