using System.Collections.Generic;
using System.Threading.Tasks;
using AuthGold.Contracts;
using AuthGold.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthGold.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RequestTraceController : ControllerBase
    {
        private readonly IRequestTrace _requestTrace;
        public RequestTraceController([FromServices] IRequestTrace requestTrace)
        {
            _requestTrace = requestTrace;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<RequestTrace>>> GetCustomers()
        {
            var response = await _requestTrace.GetAll();
            return Ok(response);
        }

        [HttpPost("")]
        public async Task<ActionResult<RequestTrace>> AddRequestTrace([FromBody] RequestTrace requestTrace)
        {
            try
            {
                await _requestTrace.Create(requestTrace);
                return Ok(requestTrace);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
