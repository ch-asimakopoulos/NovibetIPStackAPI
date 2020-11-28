using NovibetIPStackAPI.Core.Models.IPRelated.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovibetIPStackAPI.WebApi.Services.Interfaces
{
    /// <summary>
    /// An interface of a service enabling our API users to perform batch update operations.
    /// </summary>
    public interface IBatchUpdateService
    {
        /// <summary>
        /// Allows a user to update IP Details in batches.
        /// </summary>
        /// <param name="ipDetails">An array of IP detail objects, including their IP that will be updated.</param>
        /// <returns>The unique identifier of the particular BatchUpdate.</returns>
        Guid BatchUpdateDetails(IPDetailsToUpdateDTO[] ipDetails);
    }
}
