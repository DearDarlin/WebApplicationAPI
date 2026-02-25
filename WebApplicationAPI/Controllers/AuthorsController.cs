using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Abstracts;
using WebApplicationAPI.DAL.Entities;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public AuthorsController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpGet("authors")]
        public IActionResult GetAll()
        {
            var authors = _libraryService.GetAllAuthors();
            return Ok(authors);
        }

        [HttpPost("author")]
        public IActionResult Add([FromBody] AuthorDTO authorDto)
        {
            if (authorDto == null) return BadRequest();

            var author = new Author
            {
                FirstName = authorDto.FirstName,
                LastName = authorDto.LastName,
                BirthDate = authorDto.BirthDate
            };

            bool success = _libraryService.AddAuthor(author);

            if (!success)
            {
                return BadRequest(new { message = "There is such an author in the library!" });
            }

            return Ok(new { message = $"Author {author.FullName} added successfully!" });
        }

        [HttpPost("book")]
        public IActionResult AddBook([FromBody] BookDTO bookDto)
        {
            if (bookDto == null) return BadRequest();

            var book = new Book
            {
                Title = bookDto.Title,
                ISBN = bookDto.ISBN,
                PublishYear = bookDto.PublishYear,
                Price = bookDto.Price,
                AuthorId = bookDto.AuthorId

            };
            bool success = _libraryService.AddBook(book);
            if (!success)
            {
                return BadRequest(new { message = "There is such a book in the library!" });
            }
            return Ok(new { message = $"Book {book.Title} added successfully!" });
        }
    }
}
