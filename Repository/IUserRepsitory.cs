using bookpj.Entities;

namespace bookpj.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
        void Delete(User user);
        Task<bool> SaveChangesAsync();
    }
}
