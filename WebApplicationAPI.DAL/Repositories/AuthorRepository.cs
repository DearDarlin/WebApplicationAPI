using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.DAL.Abstracts;
using WebApplicationAPI.DAL.Entities;

namespace WebApplicationAPI.DAL.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _db;

        public AuthorRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(Author author)
        {
            _db.Authors.Add(author);
            _db.SaveChanges();
        }

        public List<Author> GetAll()
        {
            return _db.Authors
                .Include(a => a.Books)
                .OrderBy(a => a.FirstName)
                .ToList();
        }

        public Author GetById(int id)
        {
            return _db.Authors.Find(id);
        }

        public void Update(Author author)
        {
            _db.Authors.Update(author);
            _db.SaveChanges();
        }

        public bool IsDuplicate(string firstName, string lastName)
        {
            return _db.Authors.Any(a =>
                a.FirstName == firstName &&
                a.LastName == lastName);
        }

        public void Delete(int id)
        {
            var author = _db.Authors.Find(id);
            if (author != null)
            {
                _db.Authors.Remove(author);
                _db.SaveChanges();
            }
        }
    }
}
