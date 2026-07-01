using bookpj.DTO;
using bookpj.Entities;
using bookpj.Repository;
using bookpj.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace bookpj.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<List<BookDTO>> GetAllAsync()
        {
            try
            {
                var books = await _bookRepository.GetAllAsync();

                return books.Select(x => new BookDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.Author,
                    Price = x.Price,
                    IsAvailable = x.IsAvailable,
                    BorrowedAT = x.BorrowedAT,
                    DueDate = x.DueDate
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra tại GetAllAsync trong BookService");
                throw;
            }
        }

        public async Task<BookDTO?> GetByIdAsync(int id)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(id);
                if (book == null) return null;

                return new BookDTO
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Price = book.Price,
                    IsAvailable = book.IsAvailable,
                    BorrowedAT = book.BorrowedAT,
                    DueDate = book.DueDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra tại GetByIdAsync với ID: {Id}", id);
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreateBookDTO dto)
        {
            var dtoJson = JsonSerializer.Serialize(dto);
            try
            {
                if (dto.Price < 0) throw new ArgumentException("Giá sách không được nhỏ hơn 0");

                var book = new Book
                {
                    Title = dto.Title,
                    Author = dto.Author,
                    Price = dto.Price,
                    IsAvailable = true,
                    BorrowedAT = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14) 
                };

                await _bookRepository.AddAsync(book);
                return await _bookRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi tạo sách mới");
                throw new Exception(ex.ToString());
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateBookDTO dto)
        {
            var dtoJson = JsonSerializer.Serialize(dto);
            try
            {
                var book = await _bookRepository.GetByIdAsync(id);
                if (book == null) return false;

                if (dto.Price < 0) throw new ArgumentException("Giá sách không hợp lệ");

                book.Title = dto.Title;
                book.Author = dto.Author;
                book.Price = dto.Price;
                book.IsAvailable = dto.IsAvailable;
                book.BorrowedAT = dto.BorrowedAT;
                book.DueDate = dto.DueDate;

                _bookRepository.Update(book);
                return await _bookRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi cập nhật sách ID: {Id}", id);
                throw new Exception(ex.ToString());
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(id);
                if (book == null) return false;

                _bookRepository.Delete(book);
                return await _bookRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi xóa sách ID: {Id}", id);
                throw new Exception(ex.ToString());
            }
        }
    }
}