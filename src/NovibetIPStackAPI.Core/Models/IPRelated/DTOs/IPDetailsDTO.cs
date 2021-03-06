﻿using System.Text.Json.Serialization;
using NovibetIPStackAPI.Core.Interfaces.IPRelated;

namespace NovibetIPStackAPI.Core.Models.IPRelated.DTOs
{
    /// <summary>
    /// The class that implements the IPDetails interface. Has the geolocation details deriving from an IP address.
    /// </summary>
    public class IPDetailsDTO : IPDetails
    {
        public string City { get; set; }

        [JsonPropertyName("country_name")]
        public string Country { get; set; }

        [JsonPropertyName("continent_name")]
        public string Continent { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        /// <summary>
        /// This has to be here because of the interface contract.
        /// </summary>
        /// <returns></returns>
        public IPDetailsDTO MapToDTO()
        {
            return this;
        }

    }
}
