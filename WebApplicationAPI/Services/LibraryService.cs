using WebApplicationAPI.DAL.Abstracts;
using WebApplicationAPI.DAL.Entities;
using WebApplicationAPI.Abstracts;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Services
{
    public class LibraryService : ILibraryService
    {
        // Репозиторії для доступу до таблиць авторів та книг
        private readonly IAuthorRepository _authors;
        private readonly IBookRepository _books;

        // Конструктор з впровадженням залежностей
        public LibraryService(IAuthorRepository authors, IBookRepository books)
        {
            _authors = authors;
            _books = books;
        }

        // Отримання всіх авторів з бази даних
        public IList<Author> GetAllAuthors()
        {
            return _authors.GetAll();
        }

        // Додавання нового автора, перевірка на дублікати
        public bool AddAuthor(Author author)
        {
            if (_authors.IsDuplicate(author.FirstName, author.LastName))
            {
                return false;
            }

            _authors.Add(author);
            return true;
        }

        // Додавання нової книги, перевірка на дублікати
        public bool AddBook(Book book)
        {
            if (_books.IsDuplicate(book.Title, book.AuthorId))
            {
                return false;
            }
            _books.Add(book);
            return true;
        }

        // Метод для пошуку та фільтрації книг за різними критеріями
        public List<Book> GetFilteredBooks(string title, int? year, int? authorId, string sortOrder)
        {
            var query = _books.GetAll();

            if (!string.IsNullOrWhiteSpace(title))
            {
                // Прибираємо StringComparison для сумісності з EF
                query = query.Where(b => b.Title.Contains(title));
            }

            if (year.HasValue)
            {
                query = query.Where(b => b.PublishYear == year.Value);
            }

            if (authorId.HasValue)
            {
                query = query.Where(b => b.AuthorId == authorId.Value);
            }

            query = sortOrder switch
            {
                "title_asc" => query.OrderBy(b => b.Title),
                "title_desc" => query.OrderByDescending(b => b.Title),
                "year_asc" => query.OrderBy(b => b.PublishYear),
                "year_desc" => query.OrderByDescending(b => b.PublishYear),
                _ => query.OrderBy(b => b.Title)
            };

            return query.ToList();
        }

        // Видалення автора за ідентифікатором з перевіркою на наявність книг
        public bool DeleteAuthor(int authorId, out string message)
        {
            // Перевірка, чи існує автор з вказаним ідентифікатором
            var author = _authors.GetAll()
                                 .FirstOrDefault(a => a.Id == authorId);

            // Перевіряємо чи у автора є книги
            var hasBooks = _books.GetAll()
                                 .Any(b => b.AuthorId == authorId);

            if (hasBooks)
            {
                message = "You cannot delete this author because there are books associated with him";
                return false;
            }

            if (author == null)
            {
                message = "Author not found";
                return false;
            }

            _authors.Delete(authorId);

            message = "Author was successfully deleted";
            return true;
        }

        // Видалення книги за ідентифікатором
        public void DeleteBook(int id)
        {
            _books.Delete(id);
        }

        // Отримання автора за ідентифікатором та перетворення в DTO
        public AuthorDTO GetAuthorById(int id)
        {
            var author = _authors.GetById(id);
            if (author == null) return null;
            return new AuthorDTO
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                BirthDate = author.BirthDate
            };
        }

        // Оновлення інформації про автора
        public void UpdateAuthor(AuthorDTO authorDto)
        {
            var author = _authors.GetById(authorDto.Id);
            if (author != null)
            {
                author.FirstName = authorDto.FirstName;
                author.LastName = authorDto.LastName;
                author.BirthDate = authorDto.BirthDate;
                _authors.Update(author);
            }
        }

        // Отримання книги за ідентифікатором
        public Book GetBookById(int id)
        {
            return _books.GetById(id);
        }

        // Оновлення інформації про книгу
        public void UpdateBook(Book book)
        {
            // Отримання існуючої книги з репозиторію
            var thisBook = _books.GetById(book.Id);
            if (thisBook != null)
            {
                // Оновлення полів книги
                thisBook.Title = book.Title;
                thisBook.PublishYear = book.PublishYear;
                thisBook.Price = book.Price;
                thisBook.AuthorId = book.AuthorId;
                // Збереження оновлень у репозиторії
                _books.Update(thisBook);
            }
        }
    }
}
