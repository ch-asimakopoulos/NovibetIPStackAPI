using System;
using System.Runtime.Serialization;

namespace NovibetIPStackAPI.IPStackWrapper.Exceptions
{
    /// <summary>
    /// Exception thrown during the request to IPStack's API.
    /// </summary>
    public class IPServiceNotAvailableException : Exception
    {
        
        public IPServiceNotAvailableException() : base()
        {
        }

        public IPServiceNotAvailableException(string message) : base(message)
        {
        }

        public IPServiceNotAvailableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public IPServiceNotAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }


    }
}
