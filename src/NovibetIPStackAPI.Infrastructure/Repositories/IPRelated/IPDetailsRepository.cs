using Microsoft.EntityFrameworkCore;
using NovibetIPStackAPI.Core.Models.IPRelated;
using NovibetIPStackAPI.Infrastructure.Persistence.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovibetIPStackAPI.Infrastructure.Persistence;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.IPRelated;

namespace NovibetIPStackAPI.Infrastructure.Repositories.IPRelated
{
    /// <summary>
    /// An implementation of the IIPDetailsRepository interface. This repository is specific to the IPDetails model and includes a lot of CRUD operations that can be applied.
    /// </summary>
    public class IPDetailsRepository : IIPDetailsRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly EntityFrameworkRepository<IPDetailsModel> _efRepository;
        public IPDetailsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _efRepository = new EntityFrameworkRepository<IPDetailsModel>(dbContext);
        }

        public IPDetailsModel GetByIPAddress(string ip)
        {
            return _dbContext.IPDetails.SingleOrDefault(det => det.IP == ip);
        }
        public Task<IPDetailsModel> GetByIPAddressAsync(string ip)
        {
            return _dbContext.IPDetails.SingleOrDefaultAsync(det => det.IP == ip);
        }

        public IPDetailsModel GetById(long id)
        {
            return _efRepository.GetById(id);
        }
        public Task<IPDetailsModel> GetByIdAsync(long id)
        {
            return _efRepository.GetByIdAsync(id);
        }
        public List<IPDetailsModel> List()
        {
            return _efRepository.List();
        }
        public Task<List<IPDetailsModel>> ListAsync()
        {
            return _efRepository.ListAsync();
        }
        public async Task<IPDetailsModel> AddAsync(IPDetailsModel entity)
        {
            return await _efRepository.AddAsync(entity);

        }
        public async Task UpdateAsync(IPDetailsModel entity)
        {
            await _efRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(IPDetailsModel entity)
        {
            await _efRepository.DeleteAsync(entity);
        }

        public async Task DeleteByIdAsync(long id)
        {
            await _efRepository.DeleteByIdAsync(id);
        }

    }
}
