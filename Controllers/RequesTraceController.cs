using System.Collections.Generic;
using System.Threading.Tasks;
using AuthGold.Contracts;
using AuthGold.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthGold.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RequesTraceController : ControllerBase
    {
        private readonly IRequestTrace _requestTrace;
        public RequesTraceController([FromServices] IRequestTrace requestTrace)
        {
            _requestTrace = requestTrace;
        }

        [HttpGet("/api/RequestTrace")]
        public async Task<ActionResult<IEnumerable<RequestTrace>>> GetCustomers()
        {
            var response = await _requestTrace.GetAll();
            return Ok(response);
        }

        [HttpPost("/api/RequestTrace")]
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
