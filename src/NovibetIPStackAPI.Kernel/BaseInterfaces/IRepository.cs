using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NovibetIPStackAPI.Kernel.Interfaces
{
    /// <summary>
    /// A basic repository interface, with the simple CRUD operations.
    /// </summary>
    /// <typeparam name="T">A generic class that must implement IUpdateable.</typeparam>
    public interface IRepository<T> where T : class, IUpdateable
    {
        Task<T> GetByIdAsync(long id);
        Task<List<T>> ListAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entityToUpdate);
        Task DeleteAsync(T entityToDelete);
        Task DeleteByIdAsync(long id);
    }
}
