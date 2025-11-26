using BookshopAPI.Domain.Entities;
using BookshopAPI.Domain.Enums;
using BookshopAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookshopAPI.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class BooksController : ControllerBase
        {
            
            private static readonly List<Book> _books = new();
            
            [HttpPost]
            public IActionResult CreateBook([FromBody] BookCreateDTO dtoBook)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState); // 400

                if (!Genres.ValidGenres.Contains(dtoBook.Genre))
                    return BadRequest(new { error = "Gênero inválido." }); // 400

                // title + author não podem ser duplicados
                bool exists = _books.Any(b =>
                    b.Title.Equals(dtoBook.Title, StringComparison.OrdinalIgnoreCase) &&
                    b.Author.Equals(dtoBook.Author, StringComparison.OrdinalIgnoreCase));

                if (exists)
                    return Conflict(new { error = "Já existe um livro com este título e este autor." }); // 409

                var now = DateTime.UtcNow;

                var book = new Book
                {
                    Title = dtoBook.Title,
                    Author = dtoBook.Author,
                    Genre = dtoBook.Genre,
                    Price = dtoBook.Price,
                    Stock = dtoBook.Stock,
                    CreatedAt = now,
                    UpdatedAt = null
                };

                _books.Add(book);

                var response = ToResponseDto(book);

                return CreatedAtAction(
                    nameof(GetBookById),
                    new { id = book.Id },
                    response); // 201
            }

            [HttpGet]
            public ActionResult<IEnumerable<BookResponseDTO>> GetBooks(
                [FromQuery] string? genre,
                [FromQuery] string? title)
            {
                IEnumerable<Book> query = _books;

                if (!string.IsNullOrWhiteSpace(genre))
                    query = query.Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrWhiteSpace(title))
                    query = query.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

                var result = query.Select(ToResponseDto).ToList();

                return Ok(result); // 200
            }

            [HttpGet("{id:guid}")]
            public ActionResult<BookResponseDTO> GetBookById(Guid id)
            {
                var book = _books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                    return NotFound(); // 404

                return Ok(ToResponseDto(book)); // 200
            }


            [HttpPut("{id:guid}")]
            public IActionResult UpdateBook(Guid id, [FromBody] BookUpdateDTO dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState); // 400

                if (!Genres.ValidGenres.Contains(dto.Genre))
                    return BadRequest(new { error = "Genre is invalid." }); // 400

                var book = _books.FirstOrDefault(b => b.Id == id);

                if (book == null)
                    return NotFound(); // 404

                // title + author não duplicados (ignorando o próprio livro)
                bool exists = _books.Any(b =>
                    b.Id != id &&
                    b.Title.Equals(dto.Title, StringComparison.OrdinalIgnoreCase) &&
                    b.Author.Equals(dto.Author, StringComparison.OrdinalIgnoreCase));

                if (exists)
                    return Conflict(new { error = "Um livro com este título e autor já existem." }); // 409

                book.Title = dto.Title;
                book.Author = dto.Author;
                book.Genre = dto.Genre;
                book.Price = dto.Price;
                book.Stock = dto.Stock;
                book.UpdatedAt = DateTime.UtcNow;

                // dá pra retornar 204, mas aqui vou retornar o recurso atualizado
                return Ok(ToResponseDto(book)); // 200
            }

            // DELETE /api/books/{id}
            [HttpDelete("{id:guid}")]
            public IActionResult DeleteBook(Guid id)
            {
                var book = _books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                    return NotFound(); // 404
                _books.Remove(book);
                return NoContent(); // 204
            }

            // Mapper Book -> BookResponseDto
            private static BookResponseDTO ToResponseDto(Book book)
            {
                return new BookResponseDTO
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    Price = book.Price,
                    Stock = book.Stock,
                    CreatedAt = book.CreatedAt,
                    UpdatedAt = book.UpdatedAt
                };
            }        }
    }
    
