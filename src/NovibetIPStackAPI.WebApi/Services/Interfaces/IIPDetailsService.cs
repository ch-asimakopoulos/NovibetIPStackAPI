using NovibetIPStackAPI.Core.Interfaces;

namespace NovibetIPStackAPI.WebApi.Services
{
    public interface IIPDetailsService
    {
        IPDetails GetDetails(string ip);
    }
}