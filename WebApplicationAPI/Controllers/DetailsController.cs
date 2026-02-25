using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Abstracts;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailsController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public DetailsController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpGet("{id}")]
        public ActionResult GetDetails(int id)
        {
            var author = _libraryService.GetAuthorById(id);

            if (author == null)
            {
                return NotFound(new { message = $"Author with ID {id} not found." });
            }

            var books = _libraryService.GetFilteredBooks(null, null, id, "title_asc");

            var authorDto = new AuthorDTO
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                BirthDate = author.BirthDate
            };

            var bookDtos = books.Select(b => new BookDTO
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                PublishYear = b.PublishYear,
                Price = b.Price,
                AuthorId = b.AuthorId
            }).ToList();

            return Ok(new
            {
                Author = authorDto,
                Books = bookDtos
            });
        }
    }
}
