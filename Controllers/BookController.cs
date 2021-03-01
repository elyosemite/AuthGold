using System.Collections.Generic;
using System.Threading.Tasks;
using AuthGold.Database;
using AuthGold.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AuthGold.DTO;

namespace AuthGold.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly Context _context;

        public BookController(Context context)
        {
            _context = context;
        }

        [HttpGet("/api/books")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            return await _context.Books
                .Select(x => Converters.BookItemDTO(x))
                .ToListAsync();
        }

        [HttpGet("/api/books/{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(string id)
        {
            var bookItem = await _context.Books.FindAsync(id);

            if(bookItem == null)
            {
                return NotFound();
            }

            return Converters.BookItemDTO(bookItem);
        }

        [HttpPut("/api/books/{id}")]
        public async Task<IActionResult> PutBook(string id, [FromBody] BookDTO bookDTO)
        {
            if (id != bookDTO.ID)
            {
                return BadRequest();
            }

            var bookItem = await _context.Books.FindAsync(id);
            if(bookItem == null)
            {
                return NotFound();
            }

            bookItem.Name = bookDTO.Name;
            bookItem.Author = bookDTO.Author;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!BookItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("/api/books")]
        public async Task<ActionResult<BookDTO>> PostBook(BookDTO bookDTO)
        {
            var bookItem = new Book
            {
                ID = bookDTO.ID,
                Name = bookDTO.Name,
                Author = bookDTO.Author
            };
            _context.Books.Add(bookItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetBook),
                new { id = bookItem.ID },
                Converters.BookItemDTO(bookItem));
        }

        [HttpDelete("/api/books/{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var bookItem = await _context.Books.FindAsync(id);
            if(bookItem == null)
            {
                return NotFound();
            }

            _context.Books.Remove(bookItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool BookItemExists(string id) =>
            _context.Books.Any(e => e.ID == id);
    }
}
