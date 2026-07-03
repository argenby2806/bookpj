using bookpj.Client.HttpServices;
using Microsoft.AspNetCore.Mvc;

namespace bookpj.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Đường dẫn gốc sẽ là: api/BookUser
    public class BookUserController : ControllerBase
    {
        private readonly BookUserHttpClient _bookUserHttpClient;

        public BookUserController(BookUserHttpClient bookUserHttpClient)
        {
            _bookUserHttpClient = bookUserHttpClient;
        }

        [HttpGet("combined-list")] // Đường dẫn đầy đủ: GET api/BookUser/combined-list
        public async Task<IActionResult> GetCombinedList()
        {
            var result = await _bookUserHttpClient.GetCombinedDataAsync();

            return Ok(result);
        }
    }
}