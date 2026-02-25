using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using WebApplicationAPI.Abstracts;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _IibraryService;

        public LibraryController(ILibraryService libraryService)
        {
            _IibraryService = libraryService;
        }

        [HttpGet]
        public ActionResult<LibraryDTO> GetLibrary([FromQuery] LibraryDTO filter)
        {
            var response = filter ?? new LibraryDTO();

            var filteredBooks = _IibraryService.GetFilteredBooks(
                response.SearchTitle,
                response.SearchYear,
                response.SelectedAuthorId,
                response.SortOrder);

            response.Books = filteredBooks.Select(b => new BookDTO
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                PublishYear = b.PublishYear,
                Price = b.Price,
                AuthorId = b.AuthorId
            }).ToList();

            var allAuthors = _IibraryService.GetAllAuthors().AsEnumerable();

            if (!string.IsNullOrEmpty(response.SearchTitle) || response.SearchYear.HasValue)
            {
                var authorIdsFromBooks = response.Books.Select(b => b.AuthorId).Distinct();
                allAuthors = allAuthors.Where(a => authorIdsFromBooks.Contains(a.Id));
            }

            if (!string.IsNullOrEmpty(response.SearchAuthorName))
            {
                allAuthors = allAuthors.Where(a => a.FullName.Contains(response.SearchAuthorName, StringComparison.OrdinalIgnoreCase));
            }

            response.Authors = allAuthors.Select(a => new AuthorDTO
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                BirthDate = a.BirthDate
            }).ToList();

            return Ok(response);
        }

        [HttpDelete("author/{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            bool deleted = _IibraryService.DeleteAuthor(id, out string msg);
            if (!deleted)
            {
                return BadRequest(new { message = msg });
            }
            return Ok(new { message = msg });

        }

        [HttpDelete("book/{id}")]
        public IActionResult DeleteBook(int id)
        {
            _IibraryService.DeleteBook(id);
            return Ok(new { message = "Book was successfully deleted" });
        }






    }


    
}
