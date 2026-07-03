using bookpj.Entities;
using bookpj.Extension;
using Microsoft.EntityFrameworkCore;
using System;

namespace bookpj.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context) => _context = context;
        public async Task<IEnumerable<User>> GetAllAsync() => await _context.Users.ToListAsync();

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddAsync(User user) => await _context.Users.AddAsync(user);
        public void Delete(User user) => _context.Users.Remove(user);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }

}
