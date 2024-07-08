using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AzureSpu221MyV.Controllers
{
    public class SearchController(IConfiguration configuration) : Controller
    {
        private readonly IConfiguration _configuration = configuration;
        public async Task<IActionResult> IndexAsync(string? searchText, string searchType = "web")
        {
            ViewData["searchText"] = searchText;
            ViewData["searchType"] = searchType;

            if (!string.IsNullOrEmpty(searchText))
            {
                var azureSection = _configuration.GetSection("azure");
                var searchSection = azureSection?.GetSection("search");
                if (searchSection == null)
                {
                    throw new Exception("Configuration load error (missing azuresetting.json'?)");
                }

                string? endpoint = searchSection.GetSection("endpoint").Value;
                string? key = searchSection.GetSection("key").Value;

                if (endpoint == null || key == null)
                {
                    throw new Exception("Configuration parse error (missing 'endpoint' or 'key')");
                }

                string? path = searchType switch
                {
                    "image" => searchSection.GetSection("imagePath").Value,
                    "news" => searchSection.GetSection("newsPath").Value,
                    "video" => searchSection.GetSection("videoPath").Value,
                    _ => searchSection.GetSection("webPath").Value
                };

                if (path == null)
                {
                    throw new Exception($"Configuration parse error (missing '{searchType}Path')");
                }

                string requestUri = $"{endpoint}{path}?q={Uri.EscapeDataString(searchText)}&count=5"; // добавляем параметр count для ограничения количества результатов

                using HttpRequestMessage request = new()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(requestUri),
                    Headers =
                    {
                        { "Ocp-Apim-Subscription-Key", key }
                    }
                };

                using HttpClient client = new();
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync();

                ViewData["result"] = JsonSerializer.Deserialize<JsonNode>(result);
            }
            

            return View();
        }
    }
}
