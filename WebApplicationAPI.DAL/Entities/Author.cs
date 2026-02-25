using System.ComponentModel.DataAnnotations;

namespace WebApplicationAPI.DAL.Entities
{
    public class Author
    {
        public int Id { get; set; }

        [Required, MinLength(2)]
        public string FirstName { get; set; }

        [Required, MinLength(2)]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public List<Book> Books { get; set; } = new();
        public string FullName => $"{FirstName} {LastName}";
    }
}
