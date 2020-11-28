using NovibetIPStackAPI.IPStackWrapper.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using NovibetIPStackAPI.Core.Interfaces.IPRelated;
using NovibetIPStackAPI.WebApi.Services;
using System.Collections.Generic;
using NovibetIPStackAPI.Core.Models.IPRelated.DTOs;
using System;

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

        // PUT api/<IPController>
        [HttpPut()]
        public IActionResult BatchUpdate(IPDetailsToUpdateDTO[] ipDetailsToUpdate)
        {
            if (!ModelState.IsValid) return BadRequest();

            Guid BatchUpdateGUID = _ipDetailService.BatchUpdateDetails(ipDetailsToUpdate);

            return Ok(BatchUpdateGUID);
        }

    }
}
