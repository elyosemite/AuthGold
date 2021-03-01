using System;
using AuthGold.Database;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AuthGold.DTO;
using System.Threading.Tasks;

namespace AuthGold.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly Context _context;
        public CustomerController(Context context) {
            _context = context;
        }

        public async Task<Action<IEquatable<CustomerDTO>>> GetCustomers()
        {
            return await _context.Customers
                .Select(x => )
        }
    }
}