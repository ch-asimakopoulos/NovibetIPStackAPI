using NovibetIPStackAPI.Kernel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovibetIPStackAPI.Core.Models.BatchRelated.DTOs
{
    /// <summary>
    /// The data transfer object that will provide the API endpoint response with information regarding the batch update job process.
    /// </summary>
    public class BatchUpdateInfoDTO
    {
        public Guid JobKey { get; set; }

        public Result BatchOperationResult { get; set; }

        public int ItemsDone { get; set; }
        public int ItemsSucceeded { get; set; }

        public string SuccessPercentage => $"{(ItemsDone == 0 ? 0 : Math.Floor(100 * ((decimal)ItemsSucceeded / (decimal)ItemsDone)))}% success rate.";

        public int ItemsLeft { get; set; }
        public string TotalItemsProcessedAndItemsLeft => $"Batch Progress: {ItemsDone} / {ItemsLeft + ItemsDone}";

        public DateTime BatchStartTimeStamp { get; set; }

        public DateTime BatchLastModifiedTimeStamp { get; set; }
        public DateTime? BatchEndTimeStamp { get; set; }

    }
}
