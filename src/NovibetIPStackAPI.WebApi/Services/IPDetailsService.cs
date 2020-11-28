using NovibetIPStackAPI.Core.Interfaces;
using NovibetIPStackAPI.Infrastructure.Persistence;
using NovibetIPStackAPI.IPStackWrapper.Services;
using NovibetIPStackAPI.IPStackWrapper.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovibetIPStackAPI.WebApi.Services
{
    public class IPDetailsService : IIPDetailsService
    {
        private readonly CachedIPDetailsRepositoryDecorator _cachedIPDetailsRepository;
        public IPDetailsService(CachedIPDetailsRepositoryDecorator cachedIPDetailsRepository)
        {
            _cachedIPDetailsRepository = cachedIPDetailsRepository;
        }

        public IPDetails GetDetails(string ip)
        {
            IPDetails details;
            try
            {
                details = _cachedIPDetailsRepository.GetByIPAddress(ip);
            }
            catch (NovibetIPStackAPI.IPStackWrapper.Exceptions.IPServiceNotAvailableException ex)
            {
                throw ex;
            }
            return details;
        }
    }
}
