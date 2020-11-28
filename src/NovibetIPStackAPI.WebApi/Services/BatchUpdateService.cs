using NovibetIPStackAPI.Core.Models.BatchRelated;
using NovibetIPStackAPI.Core.Models.IPRelated.DTOs;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.BatchRelated;
using NovibetIPStackAPI.WebApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NovibetIPStackAPI.WebApi.Services
{
    public class BatchUpdateService : IBatchUpdateService
    {

        private readonly IJobRepository _repository;
        public BatchUpdateService(IJobRepository repository)
        {
            _repository = repository;
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
                JobKey = new Guid(),
                requestJSON = requestJSON
            };

            //Task.Run()

            return _repository.AddAsync(jobModel).GetAwaiter().GetResult().JobKey;
        }
    }
}
