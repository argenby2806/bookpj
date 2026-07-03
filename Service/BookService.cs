using bookpj.DTO;
using bookpj.Entities;
using bookpj.Repository;
using System.Text.Json;

namespace bookpj.Service
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
                    BookId = x.BookId,
                    Title = x.Title,
                    Author = x.Author,
                    Price = x.Price,
                    IsAvailable = x.IsAvailable
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi lấy danh sách");
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
                    BookId = book.BookId,
                    Title = book.Title,
                    Author = book.Author,
                    Price = book.Price,
                    IsAvailable = book.IsAvailable
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi lấy danh sách với id: {Id}", id);
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
                    IsAvailable = true
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