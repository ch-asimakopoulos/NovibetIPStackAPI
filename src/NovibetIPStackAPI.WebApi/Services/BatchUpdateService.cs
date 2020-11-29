using Microsoft.Extensions.DependencyInjection;
using NovibetIPStackAPI.Core.Models.BatchRelated;
using NovibetIPStackAPI.Core.Models.IPRelated.DTOs;
using NovibetIPStackAPI.Infrastructure.BatchUpdateJob.Interfaces;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.BatchRelated;
using NovibetIPStackAPI.WebApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace NovibetIPStackAPI.WebApi.Services
{
    /// <summary>
    /// Provides batch update operations for Geolocation IP details.
    /// </summary>
    public class BatchUpdateService : IBatchUpdateService
    {

        private readonly IJobRepository _repository;
        private readonly IBatchUpdateJobTaskRunner _batchTaskRunner;
        public BatchUpdateService(IJobRepository repository, IBatchUpdateJobTaskRunner batchTaskRunner)
        {
            _repository = repository;
            _batchTaskRunner = batchTaskRunner;
        }
            
        /// <summary>
        /// Allows a user to update IP Details in batches.
        /// </summary>
        /// <param name="ipDetails">An array of IP detail objects, including their IP that will be updated.</param>
        /// <returns>The unique identifier of the particular BatchUpdate.</returns>
        public Guid BatchUpdateDetails(IPDetailsToUpdateDTO[] ipDetails)
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };


            string requestJSON = JsonSerializer.Serialize(ipDetails, options);

            JobModel jobModel = new JobModel()
            {
                BatchOperationResult = Kernel.Enums.Result.InProcess,
                ItemsDone = 0,
                ItemsLeft = ipDetails.Count(),
                ItemsSucceeded = 0,
                JobKey = Guid.NewGuid(),
                requestJSON = requestJSON
            };

            Guid jobKey = _repository.AddAsync(jobModel).GetAwaiter().GetResult().JobKey;

            Task.Factory.StartNew(() => _batchTaskRunner.ProcessBatchJob(jobKey), new CancellationToken());

            return jobKey;
        }
    }
}
