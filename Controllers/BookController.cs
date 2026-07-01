using bookpj.DTO;
using bookpj.Extensions;
using bookpj.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json; 

namespace bookpj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _service;
        private readonly ILogger<BookController> _logger;
        private readonly string _methods = nameof(BookController);

        public BookController(IBookService bookService, ILogger<BookController> logger)
        {
            _service = bookService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogRequest(_methods); 
            var result = await _service.GetAllAsync();

            _logger.LogResponse(_methods, result); 
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogRequest(_methods, id); 
            var result = await _service.GetByIdAsync(id);

            if (result == null)
            {
                _logger.LogResponse(_methods, "Not Found");
                return NotFound();
            }

            _logger.LogResponse(_methods, result);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookDTO dto)
        {
            _logger.LogRequest(_methods, dto); 

            await _service.CreateAsync(dto);

            _logger.LogResponse(_methods); 
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDTO dto)
        {
            _logger.LogRequest(_methods, new { id, dto });

            await _service.UpdateAsync(id, dto);

            _logger.LogResponse(_methods);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogRequest(_methods, id);

            await _service.DeleteAsync(id);

            _logger.LogResponse(_methods);
            return Ok();
        }
    }
}