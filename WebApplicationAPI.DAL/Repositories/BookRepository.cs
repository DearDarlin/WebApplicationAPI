using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.DAL.Abstracts;
using WebApplicationAPI.DAL.Entities;

namespace WebApplicationAPI.DAL.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _db;

        public BookRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(Book book)
        {
            _db.Books.Add(book);
            _db.SaveChanges();
        }

        public IQueryable<Book> GetAll()
        {
            return _db.Books.Include(b => b.Author);
        }

        public void Delete(int id)
        {
            var book = _db.Books.Find(id);
            if (book != null)
            {
                _db.Books.Remove(book);
                _db.SaveChanges();
            }
        }

        public bool IsDuplicate(string title, int authorId)
        {
            return _db.Books.Any(b =>
            b.Title == title && b.AuthorId == authorId);

        }

        public Book GetById(int id)
        {
            return _db.Books.Include(b => b.Author).FirstOrDefault(b => b.Id == id);
        }

        public void Update(Book book)
        {
            _db.Books.Update(book);
            _db.SaveChanges();
        }
    }
}
