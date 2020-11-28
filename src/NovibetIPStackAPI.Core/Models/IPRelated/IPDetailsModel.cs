using NovibetIPStackAPI.Core.Interfaces.IPRelated;
using NovibetIPStackAPI.Core.Models.IPRelated.DTOs;
using NovibetIPStackAPI.Kernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovibetIPStackAPI.Core.Models.IPRelated
{
    /// <summary>
    /// The domain model for the Geolocational IP details. This is the model that will be saved in the persistence layer and includes more information, such as the date it was created or last modified.
    /// </summary>
    public class IPDetailsModel : IUpdateable, IPDetails
    {
        public long Id { get; set; }
        public string IP { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Continent { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastModified { get; set; }

        /// <summary>
        /// A mapper that will map this domain model to its Data Transfer equivalent object.
        /// </summary>
        /// <returns>The equivalent DTO.</returns>
        public IPDetailsDTO MapToDTO()
        {
            return new IPDetailsDTO() { City = this.City, Continent = this.Continent, Country = this.Country, Latitude = this.Latitude, Longitude = this.Longitude };
        }

    }
}
