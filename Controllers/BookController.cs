using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthGold.Contracts;
using AuthGold.Database;
using AuthGold.DTO;
using AuthGold.Models;
using AuthGold.Providers;

namespace AuthGold.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly Context _context;
        private readonly IRequestTrace _requestTrace;
        private readonly IElapsedTime _elapsedtime;
        private readonly IJsonManipulate _jsonManipulate;

        public BookController(
            Context context,
            IRequestTrace requestTrace,
            IElapsedTime elapsedtime,
            IJsonManipulate jsonManipulate
        )
        {
            _context = context;
            _requestTrace = requestTrace;
            _elapsedtime = elapsedtime;
            _jsonManipulate = jsonManipulate;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            var response = await _context.Books
                .Select(x => Converters.BookItemDTO(x))
                .ToListAsync();
            
            WriteJsonProvider.CreateFile(@"C:\Users\Patricia\Documents\Teste.y");

            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(string id)
        {
            var bookItem = await _context.Books.FindAsync(id);
            
            if(bookItem == null)
            {
                return NotFound();
            }

            var response = Converters.BookItemDTO(bookItem);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PutBook(string id, [FromBody] BookDTO bookDTO)
        {
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
            var Id = Guid.NewGuid().ToString();
            var bookItem = new Book
            {
                ID = Id,
                Name = bookDTO.Name,
                Author = bookDTO.Author,
                Secret = secret
            };

            if(ModelState.IsValid)
            {
                _context.Books.Add(bookItem);
                await _context.SaveChangesAsync();

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
            
            _context.Books.Remove(bookItem);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        private bool BookItemExists(string id) =>
            _context.Books.Any(e => e.ID == id);
    }
}
