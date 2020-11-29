using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovibetIPStackAPI.Core.Models.BatchRelated.DTOs;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.BatchRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovibetIPStackAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobProcessController : ControllerBase
    {
        private readonly IJobRepository _repository;

        public JobProcessController(IJobRepository repository)
        {
            _repository = repository;
        }

        // GET api/<JobProcessController>/5
        [HttpGet("{jobKey}")]
        public IActionResult GetJobProcess(Guid jobKey)
        {
            BatchUpdateInfoDTO batchUpdateInfo = _repository.GetByJobKey(jobKey)?.MapToDTO();

            if (batchUpdateInfo == null)
            {
                return NotFound($"There is no batch update job with job key: {jobKey}");
            }

            return Ok(batchUpdateInfo);
        }
    }
}
