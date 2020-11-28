using NovibetIPStackAPI.Core.Models.DTOs;

namespace NovibetIPStackAPI.Core.Interfaces
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

        IPDetailsDTO MapToDTO();
    }
}
