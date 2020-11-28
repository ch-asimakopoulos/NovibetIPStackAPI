using NovibetIPStackAPI.Kernel.Enums;
using System;

namespace NovibetIPStackAPI.Core.Interfaces.BatchRelated
{
    public interface IJob
    {
        Result BatchOperationResult { get; set; }
        DateTime? DateEnded { get; set; }
        int ItemsDone { get; set; }
        int ItemsLeft { get; set; }
        int ItemsSucceeded { get; set; }
        Guid JobKey { get; set; }
        string requestJSON { get; set; }
    }
}