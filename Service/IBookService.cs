using bookpj.DTO;

namespace bookpj.Service
{
    public interface IBookService
    {
        Task<List<BookDTO>> GetAllAsync();
        Task<BookDTO?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateBookDTO dto); 
        Task<bool> UpdateAsync(int id, UpdateBookDTO dto); 
        Task<bool> DeleteAsync(int id); 
    }
}
