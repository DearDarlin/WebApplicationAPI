using WebApplicationAPI.DAL.Entities;

namespace WebApplicationAPI.DAL.Abstracts
{
    public interface IAuthorRepository
    {
        List<Author> GetAll();
        void Add(Author author);
        Author GetById(int id);
        void Update(Author author);
        void Delete(int id);
        bool IsDuplicate(string firstName, string lastName);

    }
}
