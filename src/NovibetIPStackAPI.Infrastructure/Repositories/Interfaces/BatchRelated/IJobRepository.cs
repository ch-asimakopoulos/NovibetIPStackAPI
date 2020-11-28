using NovibetIPStackAPI.Core.Models.BatchRelated;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.BatchRelated
{
    /// <summary>
    /// A batch update job specific repository interface.
    /// </summary>
    public interface IJobRepository
    {
        Task<JobModel> AddAsync(JobModel entity);
        Task DeleteAsync(JobModel entity);
        Task DeleteByIdAsync(long id);
        JobModel GetById(long id);
        Task<JobModel> GetByIdAsync(long id);
        JobModel GetByJobKey(Guid jobKey);
        Task<JobModel> GetByJobKeyAsync(Guid jobKey);
        List<JobModel> List();
        Task<List<JobModel>> ListAsync();
        Task UpdateAsync(JobModel entity);
    }
}