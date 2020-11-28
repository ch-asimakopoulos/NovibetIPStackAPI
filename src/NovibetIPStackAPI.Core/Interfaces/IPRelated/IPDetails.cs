using NovibetIPStackAPI.Core.Models.IPRelated.DTOs;

namespace NovibetIPStackAPI.Core.Interfaces.IPRelated
{
    /// <summary>
    /// Interface that represents the geolocation details that derive from an IP address.
    /// </summary>
    public interface IPDetails
    {
        string City { get; set; }
        string Country { get; set; }
        string Continent { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }

        /// <summary>
        /// Whatever class implements this must be able to map itself to the data transfer object we will use in our presentation (WebAPI) layer
        /// </summary>
        /// <returns></returns>
        IPDetailsDTO MapToDTO();
    }
}
