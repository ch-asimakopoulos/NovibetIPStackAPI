using NovibetIPStackAPI.Core.Models.IPRelated;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NovibetIPStackAPI.Infrastructure.Persistence.Caching.Interfaces
{
    /// <summary>
    /// This interface creates a contract for the implementation of a decorator pattern, in order to allow us to retrieve items from an in-memory cache, a persistence repository or the IP Stack API.
    /// </summary>
    public interface ICachedIPDetailsRepositoryDecorator
    {
        Task<IPDetailsModel> AddAsync(IPDetailsModel entity);
        Task DeleteAsync(IPDetailsModel entity);
        Task DeleteByIdAsync(long id);
        IPDetailsModel GetById(long id);
        Task<IPDetailsModel> GetByIdAsync(long id);
        IPDetailsModel GetByIPAddress(string ip);
        Task<IPDetailsModel> GetByIPAddressAsync(string ip);
        List<IPDetailsModel> List();
        Task<List<IPDetailsModel>> ListAsync();
        Task UpdateAsync(IPDetailsModel entity);
    }
}