using NovibetIPStackAPI.IPStackWrapper.Models.Interfaces;
using NovibetIPStackAPI.IPStackWrapper.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IPController : ControllerBase
    {

        private readonly IIPInfoProvider _provider;
        public IPController(IIPInfoProvider provider)
        {
            _provider = provider;
        }
        // GET api/<IPController>/5
        [HttpGet("{ip}")]
        public IActionResult Get(string ip)
        {
            IPDetails a;
            try
            {
                a = _provider.GetDetails(ip);
            }
            catch (NovibetIPStackAPI.IPStackWrapper.Exceptions.IPServiceNotAvailableException ex)
            {
                return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(a);

        }

    }
}
