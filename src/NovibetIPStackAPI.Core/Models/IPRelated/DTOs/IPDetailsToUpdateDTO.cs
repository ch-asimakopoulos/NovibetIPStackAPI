using System.Text.Json.Serialization;


namespace NovibetIPStackAPI.Core.Models.IPRelated.DTOs
{
    /// <summary>
    /// A model that defines the request object needed in order to do a batch update request on our Web API.
    /// </summary>
    public class IPDetailsToUpdateDTO
    {
        public string IpAddress { get; set; }
        public string City { get; set; }

        [JsonPropertyName("country_name")]
        public string Country { get; set; }

        [JsonPropertyName("continent_name")]
        public string Continent { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

    }
}
