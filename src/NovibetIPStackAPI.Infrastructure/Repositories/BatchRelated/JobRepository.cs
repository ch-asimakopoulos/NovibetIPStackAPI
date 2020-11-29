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
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.BatchRelated;
using NovibetIPStackAPI.Core.Models.BatchRelated;

namespace NovibetIPStackAPI.Infrastructure.Repositories.BatchRelated
{
    /// <summary>
    /// An implementation of the IJobRepository interface. This repository is specific to the Job model and includes a lot of CRUD operations that can be applied.
    /// </summary>
    public class JobRepository : IJobRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly EntityFrameworkRepository<JobModel> _efRepository;
        public JobRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _efRepository = new EntityFrameworkRepository<JobModel>(dbContext);
        }

        public JobModel GetByJobKey(Guid jobKey)
        {
            return _dbContext.Jobs.SingleOrDefault(j => j.JobKey == jobKey);
        }
        public Task<JobModel> GetByJobKeyAsync(Guid jobKey)
        {
            return _dbContext.Jobs.SingleOrDefaultAsync(j => j.JobKey == jobKey);
        }

        public JobModel GetById(long id)
        {
            return _efRepository.GetById(id);
        }
        public Task<JobModel> GetByIdAsync(long id)
        {
            return _efRepository.GetByIdAsync(id);
        }
        public List<JobModel> List()
        {
            return _efRepository.List();
        }
        public Task<List<JobModel>> ListAsync()
        {
            return _efRepository.ListAsync();
        }
        public async Task<JobModel> AddAsync(JobModel entity)
        {
            return await _efRepository.AddAsync(entity);

        }
        public async Task UpdateAsync(JobModel entity)
        {
            await _efRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(JobModel entity)
        {
            await _efRepository.DeleteAsync(entity);
        }

        public async Task DeleteByIdAsync(long id)
        {
            await _efRepository.DeleteByIdAsync(id);
        }

    }
}
