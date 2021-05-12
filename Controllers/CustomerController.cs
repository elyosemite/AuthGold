using Microsoft.AspNetCore.Mvc;
using AuthGold.Contracts;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthGold.Models;
using System;
using Microsoft.AspNetCore.Http.Extensions;

namespace AuthGold.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomer _customer;
        private readonly IElapsedTime _elapsedTime;
        private readonly IRequestTrace _requestTrace;

        public CustomerController(IRequestTrace requestTrace, ICustomer customer, IElapsedTime elapsedtime) {
            _requestTrace = requestTrace;
            _customer= customer;
            _elapsedTime = elapsedtime;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAll()
        {
            var stopwatch = _elapsedTime.Open();
            stopwatch.Start();

            var response = await _customer.GetAll();
            var elapsedtime = _elapsedTime.Close(stopwatch);

            await _requestTrace.Create(new RequestTrace
            {
                id = Guid.NewGuid().ToString(),
                address = UriHelper.GetDisplayUrl(Request),
                clientCode = Guid.NewGuid().ToString(),
                elapsedTime = elapsedtime,
                httpMethod = Request.Method,
                httpStatusCode = Response.StatusCode,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            return response;
            
        }

    }
}