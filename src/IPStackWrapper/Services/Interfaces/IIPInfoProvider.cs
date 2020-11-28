using NovibetIPStackAPI.IPStackWrapper.Models.Interfaces;
using NovibetIPStackAPI.IPStackWrapper.Exceptions;

namespace NovibetIPStackAPI.IPStackWrapper.Services.Interfaces
{

    /// <summary>
    /// Resources implementing this interface will be able to get the geolocation details that derive from an IP address.
    ///<para>Possible concrete classes:</para>
    /// <list type="bullet">
    /// <item>
    /// <description><see cref="IPInfoProvider" /></description>
    /// </item>
    /// </summary>
    /// <exception cref="IPServiceNotAvailableException">Thrown if the request fails.</exception>
    public interface IIPInfoProvider
    {
        /// <summary>
        /// Gets the geolocation details that derive from the specified IP address.
        /// </summary>
        /// <param name="ip">An IP address</param>
        /// <returns>An instance of  an object that implements the <see cref="IPDetails"/> interface.</returns>
        IPDetails GetDetails(string ip);
    }
}
