using NovibetIPStackAPI.Core.Interfaces.IPRelated;
using NovibetIPStackAPI.Core.Models.IPRelated.DTOs;
using System;

namespace NovibetIPStackAPI.WebApi.Services
{
    /// <summary>
    /// An interface of a service enabling our API users to perform certain operations.
    /// </summary>
    public interface IIPDetailsService
    {
        /// <summary>
        /// Returns the Geolocational details that derive from an IP address.
        /// </summary>
        /// <param name="ip">The ip address whose details we will search for.</param>
        /// <returns>A <see cref="IPDetails"/> interface implementation instance.</returns>
        IPDetails GetDetails(string ip);

    }
}