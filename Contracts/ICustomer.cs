using System.Collections.Generic;
using System.Threading.Tasks;
using AuthGold.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthGold.Contracts
{
    public interface ICustomer
    {
        Task<ActionResult<IEnumerable<Customer>>> GetAll();
        Task<ActionResult<Customer>> Create(Customer customer);
    }
}
