using System.Collections.Generic;
using System.Threading.Tasks;
using AuthGold.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthGold.Contracts
{
    public interface IRequestTrace
    {
        Task<ActionResult<IEnumerable<RequestTrace>>> GetAll();
        Task<ActionResult<RequestTrace>> Create(RequestTrace requestTrace);
    }
}
