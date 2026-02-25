using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApplicationAPI.DAL.Entities
{
    public class Book
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

        [ForeignKey("AuthorId")]
        public Author? Author { get; set; }
    }
}
