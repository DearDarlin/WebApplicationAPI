using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplicationAPI.DAL.Entities;

namespace WebApplicationAPI.Models
{
    public class LibraryDTO
    {
        public List<AuthorDTO> Authors { get; set; } = new();
        public List<BookDTO> Books { get; set; } = new();

        public string? SearchTitle { get; set; }
        public int? SearchYear { get; set; }
        public int? SelectedAuthorId { get; set; }
        public string? SortOrder { get; set; }

        public string? Message { get; set; }

        public string? SearchAuthorName { get; set; }
        public string? AuthorSortOrder { get; set; }
    }

    public class AuthorDTO
    {
        public int Id { get; set; }

        [Required, MinLength(2)]
        public string FirstName { get; set; }

        [Required, MinLength(2)]
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }

    public class BookDTO
    {
        public int Id { get; set; }

        [Required, MinLength(2)]
        public string Title { get; set; }

        [Required]
        [RegularExpression(@"978-\d{10}")]
        public string ISBN { get; set; }

        [Required]
        [Range(1450, 2100)]
        public int PublishYear { get; set; }

        [Range(0, 100000)]
        public decimal? Price { get; set; }

        [Required]
        public int AuthorId { get; set; }
    }
}
