using System.Collections.Generic;
using System.Threading.Tasks;
using AuthGold.Contracts;
using AuthGold.Database;
using AuthGold.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthGold.Providers
{
    public class RequestTraceProvider : IRequestTrace
    {
        private readonly Context _context;
        public RequestTraceProvider(Context context)
        {
            _context = context;
        }

        public async Task<ActionResult<RequestTrace>> Create(RequestTrace requestTrace)
        {
            try
            {
                var result = _context.RequestTrace.Add(requestTrace);
                await _context.SaveChangesAsync();
                return requestTrace;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ActionResult<IEnumerable<RequestTrace>>> GetAll()
        {
            try
            {
                var response = await _context.RequestTrace.ToListAsync();
                return response;
            }
            catch
            {
                throw;
            }
        }
    }
}
