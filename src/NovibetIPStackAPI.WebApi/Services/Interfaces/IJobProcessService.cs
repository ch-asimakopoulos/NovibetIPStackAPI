using NovibetIPStackAPI.Core.Models.BatchRelated.DTOs;
using System;

namespace NovibetIPStackAPI.WebApi.Services.Interfaces
{
    /// <summary>
    /// An interface of a service that allows users to check the process of a particular batch update request job.
    /// </summary>
    /// <param name="jobKey">The unique identifier of the batch update request job.</param>
    /// <returns>Information regarding the process of the batch request job.</returns>
    public interface IJobProcessService
    {
        /// <summary>
        /// Allows a user to check the process of a particular batch update request job.
        /// </summary>
        /// <param name="jobKey">The unique identifier of the batch update request job.</param>
        /// <returns>Information regarding the process of the batch request job.</returns>
        BatchUpdateInfoDTO GetJobProcessInfo(Guid jobKey);
    }
}