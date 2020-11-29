using NovibetIPStackAPI.Core.Interfaces.IPRelated;
using NovibetIPStackAPI.Infrastructure.Persistence.Caching.Interfaces;
using NovibetIPStackAPI.IPStackWrapper.Exceptions;

namespace NovibetIPStackAPI.WebApi.Services
{
    public class IPDetailsService : IIPDetailsService
    {
        private readonly ICachedIPDetailsRepositoryDecorator _cachedIPDetailsRepository;
        public IPDetailsService(ICachedIPDetailsRepositoryDecorator cachedIPDetailsRepository)
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
            catch (IPServiceNotAvailableException ex)
            {
                throw ex;
            }
            return details;
        }
    }
}
