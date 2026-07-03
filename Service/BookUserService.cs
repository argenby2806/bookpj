using System.Net.Http.Json;
using bookpj.DTO;

namespace bookpj.Client.HttpServices
{
    public class BookUserHttpClient
    {
        private readonly HttpClient _httpClient;

        public BookUserHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BookUserDto>> GetCombinedDataAsync()
        {
            try
            {
                var books = await _httpClient.GetFromJsonAsync<List<BookDTO>>("api/book");

                var users = await _httpClient.GetFromJsonAsync<List<UserDTO>>("api/user");

                var combinedList = new List<BookUserDto>();

                if (books != null && users != null)
                {
                    int count = Math.Min(books.Count, users.Count);

                    for (int i = 0; i < count; i++)
                    {
                        combinedList.Add(new BookUserDto
                        {
                            Title = books[i].Title,       
                            Username = users[i].Username  
                        });
                    }
                }

                return combinedList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gọi song song 2 API: {ex.Message}");
                return new List<BookUserDto>();
            }
        }
    }
}