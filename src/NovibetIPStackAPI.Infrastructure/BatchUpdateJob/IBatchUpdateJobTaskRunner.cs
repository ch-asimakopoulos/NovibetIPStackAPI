using System;

namespace NovibetIPStackAPI.Infrastructure.BatchUpdateJob.Interfaces
{
    /// <summary>
    /// The interface of an implementation that allows updating IP Geolocation details in batches, by utilizing the Task Asynchronous Pattern (TAP) design pattern.
    /// </summary>
    public interface IBatchUpdateJobTaskRunner
    {
        /// <summary>
        /// Processes the batch job in hand.
        /// </summary>
        /// <param name="jobKey">The unique identifier of the job that needs to be processed.</param>
        void ProcessBatchJob(Guid jobKey);
    }
}