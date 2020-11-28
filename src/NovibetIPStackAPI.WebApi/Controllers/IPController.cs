using NovibetIPStackAPI.IPStackWrapper.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using NovibetIPStackAPI.Core.Interfaces;
using NovibetIPStackAPI.WebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IPController : ControllerBase
    {

        private readonly IIPDetailsService _ipDetailService;
        public IPController(IIPDetailsService ipDetailService)
        {
            _ipDetailService = ipDetailService;
        }
        // GET api/<IPController>/5
        [HttpGet("{ip}")]
        public IActionResult Get(string ip)
        {
            IPDetails detailsForThisIp; 
            try
            {
                detailsForThisIp = _ipDetailService.GetDetails(ip);
            }
            catch (NovibetIPStackAPI.IPStackWrapper.Exceptions.IPServiceNotAvailableException ex)
            {
                return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
            }

            return Ok(detailsForThisIp.MapToDTO());

        }

    }
}
