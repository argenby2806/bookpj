using bookpj.Entities;
using bookpj.Extension;
using Microsoft.EntityFrameworkCore;
using System;

namespace bookpj.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _context; 

        public BookRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
        }

        public void Update(Book book)
        {
            _context.Books.Entry(book).State = EntityState.Modified;
        }

        public void Delete(Book book)
        {
            _context.Books.Remove(book);
        }
        public async Task<bool> SaveChangesAsync() 
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
