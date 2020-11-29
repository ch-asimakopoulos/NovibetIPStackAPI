using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    /// Allows updating IP Geolocation details in batches, by utilizing the unit of work design pattern.
    /// </summary>
    public class BatchUpdateJobUnitOfWork : IBatchUpdateJobUnitOfWork
    {
        private IJobRepository _jobRepository;
        private IIPDetailsRepository _ipDetailsRepository;
        private readonly int _processedItemsPerBatch;
        private readonly IServiceScopeFactory _iScopeFactory;
        public BatchUpdateJobUnitOfWork(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _iScopeFactory = serviceScopeFactory;
            if (!int.TryParse(configuration["BatchUpdateJobOptions:ItemsPerBatchTransaction"], out _processedItemsPerBatch))
            {
                //TO FIX
                throw new Exception();
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
                {
                    AppDbContext appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    _jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();
                    _ipDetailsRepository = scope.ServiceProvider.GetRequiredService<IIPDetailsRepository>();
                    //IDbContextTransaction transaction = appDbContext.Database.BeginTransaction();


                    JobModel jobToProcess = _jobRepository.GetByJobKey(jobKey);

                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    IPDetailsToUpdateDTO[] itemsToProcess = JsonSerializer.Deserialize<IPDetailsToUpdateDTO[]>(jobToProcess.requestJSON, options);

                    if (itemsToProcess.Count() == 0)
                    {
                        throw new ArgumentException($"Job {jobKey} had zero items to process.");
                    }

                    try
                    {
                        foreach (int i in Enumerable.Range(0, (itemsToProcess.Count() / _processedItemsPerBatch) + 1))
                        {

                            IEnumerable<IPDetailsToUpdateDTO> BatchItemsToProcess = itemsToProcess.Skip(i * _processedItemsPerBatch).Take(_processedItemsPerBatch);

                            BatchToProcess(ref jobToProcess, BatchItemsToProcess);

                        }

                    }
                    catch (Exception ex)
                    {
                        //transaction.Rollback();
                        //Log exception
                        jobToProcess.BatchOperationResult = Kernel.Enums.Result.Failure;
                        _jobRepository.UpdateAsync(jobToProcess).GetAwaiter().GetResult();
                    }

                    SetJobCompleted(ref jobToProcess);

                    //transaction.Commit();
                }
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
                    IPDetailsModel ipDetailsModel = _ipDetailsRepository.GetByIPAddress(det.IpAddress);

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
                    //Log exception

                }
            }

            currentJob.ItemsLeft -= ipDetails.Count();
            currentJob.ItemsDone += ipDetails.Count();
            currentJob.ItemsSucceeded += successfulUpdates;


            _jobRepository.UpdateAsync(currentJob).GetAwaiter().GetResult();

        }

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

            _jobRepository.UpdateAsync(jobModel).GetAwaiter().GetResult();
        }

    }

}
