using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NovibetIPStackAPI.Core.Models.BatchRelated;
using NovibetIPStackAPI.Core.Models.IPRelated;
using NovibetIPStackAPI.Core.Models.IPRelated.DTOs;
using NovibetIPStackAPI.Infrastructure.BatchUpdateJob.Interfaces;
using NovibetIPStackAPI.Infrastructure.Persistence;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.BatchRelated;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.IPRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Transactions;

namespace NovibetIPStackAPI.Infrastructure.BatchUpdateJob
{
    /// <summary>
    /// Allows updating IP Geolocation details in batches, by utilizing the Task Asynchronous design pattern.
    /// </summary>
    public class BatchUpdateJobTaskRunner : IBatchUpdateJobTaskRunner
    {
        private IJobRepository _jobRepository;
        private IIPDetailsRepository _ipDetailsRepository;
        private readonly int _processedItemsPerBatch;
        private readonly IServiceScopeFactory _iScopeFactory;
        private List<IPDetailsModel> _ipModelsStored;
        private ILogger<BatchUpdateJobTaskRunner> _logger;
        public BatchUpdateJobTaskRunner(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, ILogger<BatchUpdateJobTaskRunner> logger)
        {
            _iScopeFactory = serviceScopeFactory;
            _logger = logger;
            if (!int.TryParse(configuration["BatchUpdateJobOptions:ItemsPerBatchTransaction"], out _processedItemsPerBatch))
            {
                _logger.LogError($"Could not find the ItemsPerBatchTransaction for the batch update job in the provided configuration.");
                throw new Exception($"Could not find the ItemsPerBatchTransaction for the batch update job in the provided configuration.");

            }

        }

        /// <summary>
        /// Processes the job specified by it's unique identifier, using a context Transaction.
        /// </summary>
        /// <param name="jobKey">The job's unique identifier.</param>
        public void ProcessBatchJob(Guid jobKey)
        {
            using (IServiceScope scope = _iScopeFactory.CreateScope())
            {

                _logger.LogDebug($"Job: {jobKey}, has entered the process batch job scope");
                _logger.LogInformation($"Job: {jobKey} processing has began.");
                AppDbContext appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                _jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();
                _ipDetailsRepository = scope.ServiceProvider.GetRequiredService<IIPDetailsRepository>();

                _ipModelsStored = _ipDetailsRepository.List();

                JobModel jobToProcess = _jobRepository.GetByJobKey(jobKey);

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                IPDetailsToUpdateDTO[] itemsToProcess = JsonSerializer.Deserialize<IPDetailsToUpdateDTO[]>(jobToProcess.requestJSON, options);

                if (itemsToProcess.Count() == 0)
                {
                    _logger.LogError($"Job {jobKey} had zero items to process.");
                    throw new ArgumentException($"Job {jobKey} had zero items to process.");
                }

                try
                {
                    foreach (int i in Enumerable.Range(0, (itemsToProcess.Count() / _processedItemsPerBatch) + 1))
                    {

                        _logger.LogDebug($"Job {jobKey} is in batch {i} out of {(itemsToProcess.Count() / _processedItemsPerBatch) + 1}");
                        IEnumerable<IPDetailsToUpdateDTO> BatchItemsToProcess = itemsToProcess.Skip(i * _processedItemsPerBatch).Take(_processedItemsPerBatch);

                        BatchToProcess(ref jobToProcess, BatchItemsToProcess);

                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Job {jobKey} aborted unexpectedly. Exception message: {ex.Message}");
                    jobToProcess.BatchOperationResult = Kernel.Enums.Result.Aborted;
                    _jobRepository.UpdateAsync(jobToProcess);
            
                }

                SetJobCompleted(ref jobToProcess);


            }
        }

        /// <summary>
        /// Processes a part of the batch.
        /// </summary>
        /// <param name="currentJob">The current job as described by it's model.</param>
        /// <param name="ipDetails">The part of the batch that will be processed.</param>
        private void BatchToProcess(ref JobModel currentJob, IEnumerable<IPDetailsToUpdateDTO> ipDetails)
        {

            int successfulUpdates = 0;

            foreach (IPDetailsToUpdateDTO det in ipDetails)
            {
                try
                {
                    IPDetailsModel ipDetailsModel = _ipModelsStored.Where(li => li.IP == det.IpAddress).FirstOrDefault();

                    ipDetailsModel.City = string.IsNullOrWhiteSpace(det.City) ? ipDetailsModel.City : det.City;
                    ipDetailsModel.Continent = string.IsNullOrWhiteSpace(det.Continent) ? ipDetailsModel.Continent : det.Continent;
                    ipDetailsModel.Country = string.IsNullOrWhiteSpace(det.Country) ? ipDetailsModel.Country : det.Country;
                    ipDetailsModel.Latitude = det.Latitude.GetValueOrDefault(ipDetailsModel.Latitude);
                    ipDetailsModel.Longitude = det.Longitude.GetValueOrDefault(ipDetailsModel.Longitude);

                    _ipDetailsRepository.UpdateAsync(ipDetailsModel).GetAwaiter().GetResult();
                    successfulUpdates++;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Job: {currentJob.JobKey} failed to update detail: {det.IpAddress}. Exception message: {ex.Message}");

                }
            }

            currentJob.ItemsLeft -= ipDetails.Count();
            currentJob.ItemsDone += ipDetails.Count();
            currentJob.ItemsSucceeded += successfulUpdates;


            _jobRepository.UpdateAsync(currentJob).GetAwaiter().GetResult();


        }

        /// <summary>
        /// Marks the process job as completed, with the correct result status depending on successes and failures.
        /// </summary>
        /// <param name="jobModel"></param>
        private void SetJobCompleted(ref JobModel jobModel)
        {
            jobModel.DateEnded = DateTime.UtcNow;

            if (jobModel.ItemsDone != 0)
            {
                if (jobModel.ItemsSucceeded == jobModel.ItemsDone)
                {
                    jobModel.BatchOperationResult = Kernel.Enums.Result.Success;
                }
                else
                {
                    if (jobModel.ItemsSucceeded > 0)
                    {
                        jobModel.BatchOperationResult = Kernel.Enums.Result.PartialSuccess;
                    }
                    else
                    {
                        jobModel.BatchOperationResult = Kernel.Enums.Result.Failure;
                    }
                }
            }
            else
            {
                jobModel.BatchOperationResult = Kernel.Enums.Result.Failure;
            }

            _jobRepository.UpdateAsync(jobModel).GetAwaiter().GetResult();

            _logger.LogInformation($"Job: {jobModel.JobKey} is completely processed.");
        }

    }

}
