using NovibetIPStackAPI.Core.Models.BatchRelated.DTOs;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.BatchRelated;
using NovibetIPStackAPI.WebApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovibetIPStackAPI.WebApi.Services
{
    public class JobProcessService : IJobProcessService
    {
        private readonly IJobRepository _repository;
        public JobProcessService(IJobRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Allows a user to check the process of a particular batch update request job.
        /// </summary>
        /// <param name="jobKey">The unique identifier of the batch update request job.</param>
        /// <returns>Information regarding the process of the batch request job.</returns>
        public BatchUpdateInfoDTO GetJobProcessInfo(Guid jobKey)
        {
            return _repository.GetByJobKey(jobKey)?.MapToDTO();

        }
    }
}
