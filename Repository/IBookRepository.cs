using bookpj.Entities;

namespace bookpj.Repository;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task AddAsync(Book book);
    void Update(Book book); 
    void Delete(Book book);
    Task<bool> SaveChangesAsync();
}