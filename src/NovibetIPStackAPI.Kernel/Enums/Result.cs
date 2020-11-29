using System;
using System.Collections.Generic;
using System.Text;

namespace NovibetIPStackAPI.Kernel.Enums
{
    /// <summary>
    /// An enum which signals the result of an operation. An operation can be successful, partially successful, a failure, aborted or currently in process. More could be added here.
    /// </summary>
    public enum Result
    {
        Success,
        Failure,
        PartialSuccess,
        InProcess,
        Aborted
    }
}
