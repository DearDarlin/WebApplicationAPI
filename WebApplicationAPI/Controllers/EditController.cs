using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Abstracts;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditController : ControllerBase 
    {
        private readonly ILibraryService _libraryService;

        public EditController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpGet("{id}")]
        public ActionResult<AuthorDTO> GetAuthor(int id)
        {
            var authorDto = _libraryService.GetAuthorById(id);

            if (authorDto == null)
            {
                return NotFound(new { message = $"Author with ID {id} not found." });
            }

            return Ok(authorDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateAuthor(int id, [FromBody] AuthorDTO authorDto)
        {
            if (id != authorDto.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _libraryService.UpdateAuthor(authorDto);
                return Ok(new { message = "Author data successfully updated!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating", details = ex.Message });
            }
        }




        [HttpGet("book/{id}")]
        public ActionResult<BookDTO> GetBook(int id)
        {
            var book = _libraryService.GetBookById(id);

            if (book == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            var bookDto = new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                PublishYear = book.PublishYear,
                Price = book.Price,
                AuthorId = book.AuthorId
            };

            return Ok(bookDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("book/{id}")]
        public IActionResult UpdateBook(int id, [FromBody] BookDTO bookDto)
        {
            if (id != bookDto.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            int currentYear = DateTime.Now.Year;
            if (bookDto.PublishYear < 1450 || bookDto.PublishYear > currentYear)
            {
                return BadRequest(new { message = $"The year must be between 1450 and {currentYear}" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookEntity = _libraryService.GetBookById(id);
            if (bookEntity == null) return NotFound();

            bookEntity.Title = bookDto.Title;
            bookEntity.ISBN = bookDto.ISBN;
            bookEntity.PublishYear = bookDto.PublishYear;
            bookEntity.Price = bookDto.Price;
            bookEntity.AuthorId = bookDto.AuthorId;

            _libraryService.UpdateBook(bookEntity);

            return Ok(new { message = "Changes saved successfully!" });
        }



    }
}
