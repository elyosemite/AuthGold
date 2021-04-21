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

        public BookController(Context context, IRequestTrace requestTrace)
        {
            _context = context;
            _requestTrace = requestTrace;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var response = await _context.Books
                .Select(x => Converters.BookItemDTO(x))
                .ToListAsync();
            
            stopwatch.Stop();

            var elapsed_time = stopwatch.ElapsedMilliseconds;
            TimeSpan t = TimeSpan.FromMilliseconds(elapsed_time);

            var elapsedtime = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                                t.Hours,
                                t.Minutes,
                                t.Seconds,
                                t.Milliseconds);

            await _requestTrace.Create(new RequestTrace
            {
                id = Guid.NewGuid().ToString(),
                address = UriHelper.GetDisplayUrl(Request),
                clientCode = Guid.NewGuid().ToString(),
                elapsedTime = t,
                httpMethod = Request.Method,
                httpStatusCode = Response.StatusCode,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(string id)
        {
            var stopwatch = new Stopwatch();
            var bookItem = await _context.Books.FindAsync(id);
            
            if(bookItem == null)
            {
                return NotFound();
            }

            stopwatch.Start();

            var response = Converters.BookItemDTO(bookItem);
            stopwatch.Stop();

            var elapsed_time = stopwatch.ElapsedMilliseconds;
            TimeSpan t = TimeSpan.FromMilliseconds(elapsed_time);

            var elapsedtime = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                                t.Hours,
                                t.Minutes,
                                t.Seconds,
                                t.Milliseconds);

            await _requestTrace.Create(new RequestTrace
            {
                id = Guid.NewGuid().ToString(),
                address = UriHelper.GetDisplayUrl(Request),
                clientCode = Guid.NewGuid().ToString(),
                elapsedTime = t,
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
            var stopwatch = new Stopwatch();
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

                var elapsed_time = stopwatch.ElapsedMilliseconds;
                TimeSpan t = TimeSpan.FromMilliseconds(elapsed_time);

                var elapsedtime = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds,
                                    t.Milliseconds);

                await _requestTrace.Create(new RequestTrace
                {
                    id = Guid.NewGuid().ToString(),
                    address = UriHelper.GetDisplayUrl(Request),
                    clientCode = Guid.NewGuid().ToString(),
                    elapsedTime = t,
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
            var secret = "KEY-asdfajg65h54fgjhlk";
            var stopwatch = new Stopwatch();
            var bookItem = new Book
            {
                ID = bookDTO.ID,
                Name = bookDTO.Name,
                Author = bookDTO.Author,
                Secret = secret
            };

            if(ModelState.IsValid)
            {
                stopwatch.Start();
                _context.Books.Add(bookItem);
                await _context.SaveChangesAsync();
                stopwatch.Stop();

                var elapsed_time = stopwatch.ElapsedMilliseconds;
                TimeSpan t = TimeSpan.FromMilliseconds(elapsed_time);

                var elapsedtime = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds,
                                    t.Milliseconds);

                await _requestTrace.Create(new RequestTrace
                {
                    id = Guid.NewGuid().ToString(),
                    address = UriHelper.GetDisplayUrl(Request),
                    clientCode = Guid.NewGuid().ToString(),
                    elapsedTime = t,
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
            var bookItem = await _context.Books.FindAsync(id);
            if(bookItem == null)
            {
                return BadRequest();
            }
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            _context.Books.Remove(bookItem);
            await _context.SaveChangesAsync();

            stopwatch.Stop();

            var elapsed_time = stopwatch.ElapsedMilliseconds;
            TimeSpan t = TimeSpan.FromMilliseconds(elapsed_time);

            var elapsedtime = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                                t.Hours,
                                t.Minutes,
                                t.Seconds,
                                t.Milliseconds);

            await _requestTrace.Create(new RequestTrace
            {
                id = Guid.NewGuid().ToString(),
                address = UriHelper.GetDisplayUrl(Request),
                clientCode = Guid.NewGuid().ToString(),
                elapsedTime = t,
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
