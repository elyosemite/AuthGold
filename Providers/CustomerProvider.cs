using System.Collections.Generic;
using System.Threading.Tasks;
using AuthGold.Contracts;
using AuthGold.Database;
using AuthGold.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthGold.Providers
{
    public class CustomerProvider : ICustomer
    {
        private readonly Context _context;

        public CustomerProvider(
            Context context
        )
        {
            _context = context;
        }

        public async Task<ActionResult<Customer>> Create(Customer customer)
        {
            try
            {
                var result = _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return customer;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ActionResult<IEnumerable<Customer>>> GetAll()
        {
            try
            {
                var response = await _context.Customers.ToListAsync();
                return response;
            }
            catch
            {
                throw;
            }
        }
    }
}
