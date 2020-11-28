using Microsoft.EntityFrameworkCore;
using NovibetIPStackAPI.Core.Models;
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

        public DbSet<IPDetailsModel> IPDetails { get; set; }

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
