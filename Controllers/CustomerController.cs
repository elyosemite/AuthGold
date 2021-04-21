using Microsoft.AspNetCore.Mvc;
using AuthGold.Contracts;

namespace AuthGold.Controllers
{
    [ApiController]
    [Route("v1/api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IRequestTrace _requestTrace;
        public CustomerController(IRequestTrace requestTrace) {
            _requestTrace = requestTrace;
        }
    }
}