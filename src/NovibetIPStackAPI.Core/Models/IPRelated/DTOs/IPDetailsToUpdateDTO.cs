using NovibetIPStackAPI.Core.Interfaces.IPRelated;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovibetIPStackAPI.Core.Models.IPRelated.DTOs
{
    /// <summary>
    /// A model that defines the request object needed in order to do a batch update request on our Web API.
    /// </summary>
    public class IPDetailsToUpdateDTO : IPDetailsDTO
    {
        public string IpAddress { get; set; }
        
    }
}
