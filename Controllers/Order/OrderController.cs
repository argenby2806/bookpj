using bookpj.DTO;
using bookpj.Extensions;
using bookpj.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace bookpj.Controllers.Order
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly ILogger<OrderController> _logger;
        private readonly string _methods = nameof(OrderController);

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _service = orderService;
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
        //[Authorize]
        public async Task<IActionResult> Create([FromBody] CreateOrderDTO dto)
        {
            _logger.LogRequest(_methods, dto);

            var success = await _service.CreateAsync(dto);
            if (!success)
            {
                _logger.LogResponse(_methods, "Create failed");
                return BadRequest(new { Message = "Tạo đơn hàng thất bại." });
            }

            _logger.LogResponse(_methods);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderDTO dto)
        {
            _logger.LogRequest(_methods, new { id, dto });

            var success = await _service.UpdateAsync(id, dto);
            if (!success)
            {
                _logger.LogResponse(_methods, "Not Found or update failed");
                return NotFound(new { Message = "Không tìm thấy đơn hàng hoặc cập nhật thất bại." });
            }

            _logger.LogResponse(_methods);
            return NoContent();
        }

        [HttpDelete("{id}")]
        //[Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogRequest(_methods, id);

            var success = await _service.DeleteAsync(id);
            if (!success)
            {
                _logger.LogResponse(_methods, "Not Found");
                return NotFound();
            }

            _logger.LogResponse(_methods);
            return NoContent();
        }
    }
}