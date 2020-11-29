using NovibetIPStackAPI.Core.Interfaces.BatchRelated;
using NovibetIPStackAPI.Core.Models.BatchRelated.DTOs;
using NovibetIPStackAPI.Kernel.Enums;
using NovibetIPStackAPI.Kernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovibetIPStackAPI.Core.Models.BatchRelated
{
    /// <summary>
    /// The  batch request job model which implements IUpdateable and IJob interfaces. This is the persistence layer model and contains more information such as the dates created, modified and ended and the requestJSON object.
    /// </summary>
    public class JobModel : IUpdateable, IJob
    {
        public long Id { get; set; }
        public Guid JobKey { get; set; }
        public Result BatchOperationResult { get; set; }
        public int ItemsDone { get; set; }
        public int ItemsSucceeded { get; set; }
        public int ItemsLeft { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastModified { get; set; }

        public DateTime? DateEnded { get; set; }
        public string requestJSON { get; set; }

        /// <summary>
        /// A mapper that will map this domain model to its Data Transfer equivalent object containing information regarding the batch request job.
        /// </summary>
        /// <returns>The equivalent DTO.</returns>
        public BatchUpdateInfoDTO MapToDTO()
        {
            return new BatchUpdateInfoDTO()
            {
                JobKey = this.JobKey,
                BatchOperationResult = this.BatchOperationResult,
                ItemsDone = this.ItemsDone,
                ItemsSucceeded = this.ItemsSucceeded,
                ItemsLeft = this.ItemsLeft,
                BatchStartTimeStamp = this.DateCreated,
                BatchLastModifiedTimeStamp = this.DateLastModified,
                BatchEndTimeStamp = this.DateEnded
            };
        }

    }
}
