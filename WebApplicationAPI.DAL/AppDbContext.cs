using WebApplicationAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationAPI.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureBooks(modelBuilder);
        }

        private void ConfigureBooks(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Book>();

            entity.HasIndex(b => b.ISBN)
                  .IsUnique();
        }
    }
}
