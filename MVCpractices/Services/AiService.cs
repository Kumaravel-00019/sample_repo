using System.Text.Json;

namespace MVCpractices.Services
{
    public class AiService
    {
        private readonly HttpClient _httpClient;

        public AiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAiResponse(string prompt)
        {
            try
            {
                var request = new
                {
                    model = "gemma-2b-it",
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    }
                };
                var response = await _httpClient.PostAsJsonAsync("http://localhost:1234/v1/chat/completions", request);
                var responseContent = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(responseContent);
                string? content = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                return content ?? string.Empty;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                return $"Error: {ex.Message}";
            }
        }
    }
}
