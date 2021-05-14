using System.Collections.Generic;
using System.Threading.Tasks;
using AuthGold.Database;
using AuthGold.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AuthGold.DTO;
using System;
using System.Diagnostics;
using AuthGold.Providers;
using AuthGold.Contracts;
using Microsoft.AspNetCore.Http.Extensions;

namespace AuthGold.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly Context _context;
        private readonly IRequestTrace _requestTrace;
        private readonly IElapsedTime _elapsedtime;

        public BookController(Context context, IRequestTrace requestTrace, IElapsedTime elapsedtime)
        {
            _context = context;
            _requestTrace = requestTrace;
            _elapsedtime = elapsedtime;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            var stopwatch = _elapsedtime.Open();
            stopwatch.Start();

            var response = await _context.Books
                .Select(x => Converters.BookItemDTO(x))
                .ToListAsync();

            var elapsedtime = _elapsedtime.Close(stopwatch);

            var reqTrace = new RequestTrace
            {
                id = Guid.NewGuid().ToString(),
                address = UriHelper.GetDisplayUrl(Request),
                clientCode = Guid.NewGuid().ToString(),
                elapsedTime = elapsedtime,
                httpMethod = Request.Method,
                httpStatusCode = Response.StatusCode,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _requestTrace.Create(reqTrace);

            var writeJsonProvider = new WriteJsonProvider();
            writeJsonProvider.WriteJson("C:\\Users\\Patricia\\Documents\\RequestTrace.yur", reqTrace);
            
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(string id)
        {
            var stopwatch = _elapsedtime.Open();
            stopwatch.Start();

            var bookItem = await _context.Books.FindAsync(id);
            
            if(bookItem == null)
            {
                return NotFound();
            }

            var response = Converters.BookItemDTO(bookItem);
            var elapsedtime = _elapsedtime.Close(stopwatch);

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

            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PutBook(string id, [FromBody] BookDTO bookDTO)
        {
            var stopwatch = _elapsedtime.Open();
            stopwatch.Start();

            var bookItem = await _context.Books.FindAsync(id);
            
            if(bookItem == null)
            {
                return NotFound();
            }

            var myUpdatedBook = new BookDTO()
            {
                Author = bookDTO.Author == null ? string.Empty : bookDTO.Author.ToString(),
                Name = bookDTO.Name == null ? string.Empty : bookDTO.Name.ToString()
            };

            bookItem.Name = myUpdatedBook.Name;
            bookItem.Author = myUpdatedBook.Author;

            try
            {
                await _context.SaveChangesAsync();
                stopwatch.Stop();

                var elapsedtime = _elapsedtime.Close(stopwatch);

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
                return NoContent();
            }
            catch (DbUpdateConcurrencyException) when (!BookItemExists(id))
            {
                return NotFound();
            }
        }

        [HttpPost("")]
        public async Task<ActionResult<BookDTO>> PostBook(BookDTO bookDTO)
        {
            var stopwatch = _elapsedtime.Open();
            stopwatch.Start();

            var secret = "KEY-asdfajg65h54fgjhlk";
            var bookItem = new Book
            {
                ID = bookDTO.ID,
                Name = bookDTO.Name,
                Author = bookDTO.Author,
                Secret = secret
            };

            if(ModelState.IsValid)
            {
                _context.Books.Add(bookItem);
                await _context.SaveChangesAsync();

                var elapsedtime = _elapsedtime.Close(stopwatch);

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

                return CreatedAtAction(nameof(PostBook), new { id = bookItem.ID }, Converters.BookItemDTO(bookItem));
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var stopwatch = _elapsedtime.Open();
            stopwatch.Start();
            var bookItem = await _context.Books.FindAsync(id);
            if(bookItem == null)
            {
                return BadRequest();
            }
            
            _context.Books.Remove(bookItem);
            await _context.SaveChangesAsync();

            var elapsedtime = _elapsedtime.Close(stopwatch);

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

            return NoContent();
        }

        private bool BookItemExists(string id) =>
            _context.Books.Any(e => e.ID == id);
    }
}
