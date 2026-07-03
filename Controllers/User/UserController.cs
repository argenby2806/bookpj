using bookpj.DTO;
using bookpj.Extensions;
using bookpj.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;

namespace bookpj.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<UserController> _logger;
        private readonly string _methods = nameof(UserController);

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _service = userService;
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
        public async Task<IActionResult> Create([FromBody] UserDTO dto)
        {
            _logger.LogRequest(_methods, dto);

            var result = await _service.CreateAsync(dto);

            if (!result)
            {
                _logger.LogResponse(_methods, "Bad Request - Create Failed");
                return BadRequest();
            }

            _logger.LogResponse(_methods);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDTO dto)
        {
            _logger.LogRequest(_methods, new { id, dto });
            var result = await _service.UpdateAsync(id, dto);

            if (!result)
            {
                _logger.LogResponse(_methods, "Not Found or Update Failed");
                return NotFound();
            }

            _logger.LogResponse(_methods);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogRequest(_methods, id);

            var result = await _service.DeleteAsync(id);

            if (!result)
            {
                _logger.LogResponse(_methods, "Not Found or Delete Failed");
                return NotFound();
            }

            _logger.LogResponse(_methods);
            return Ok();
        }
    }
}