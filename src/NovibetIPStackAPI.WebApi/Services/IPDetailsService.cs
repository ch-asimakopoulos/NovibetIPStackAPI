using NovibetIPStackAPI.Core.Interfaces.IPRelated;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.IPRelated;
using NovibetIPStackAPI.IPStackWrapper.Exceptions;

namespace NovibetIPStackAPI.WebApi.Services
{
    public class IPDetailsService : IIPDetailsService
    {
        private readonly IIPDetailsRepository _IIPDetailsRepository;
        public IPDetailsService(IIPDetailsRepository IIPDetailsRepository)
        {
            _IIPDetailsRepository = IIPDetailsRepository;
        }

        public IPDetails GetDetails(string ip)
        {
            IPDetails details;
            try
            {
                details = _IIPDetailsRepository.GetByIPAddress(ip);
            }
            catch (IPServiceNotAvailableException ex)
            {
                throw ex;
            }
            return details;
        }
    }
}
