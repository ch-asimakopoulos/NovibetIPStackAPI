using NovibetIPStackAPI.Kernel.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NovibetIPStackAPI.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// An entity framework specific repository. Here we can add more CRUD operations, which could include filters, paginated queries etc.
    /// </summary>
    /// <typeparam name="T">The generic class T, which must implement IUpdateable.</typeparam>
    public interface IEntityFrameworkRepository<T> : IRepository<T> where T : class, IUpdateable
    {
        T GetById(long id);
        List<T> List();
    }
}