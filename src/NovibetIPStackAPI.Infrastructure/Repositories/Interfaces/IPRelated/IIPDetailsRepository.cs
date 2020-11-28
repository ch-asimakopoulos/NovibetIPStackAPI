using NovibetIPStackAPI.Core.Models.IPRelated;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.IPRelated
{
    /// <summary>
    /// An IPDetails specific repository interface.
    /// </summary>
    public interface IIPDetailsRepository
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