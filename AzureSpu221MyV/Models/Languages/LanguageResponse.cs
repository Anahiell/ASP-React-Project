using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace AzureSpu221MyV.Models.Languages
{
    public class LanguageResponse
    {
        public Dictionary<String, LanguageData> translation { get; set; }
        public static LanguageResponse FromJson(String jsonString)
        {
            JsonNode json = JsonSerializer.Deserialize<JsonNode>(jsonString)!;
            return new()
            {
                translation = JsonSerializer
                .Deserialize<Dictionary<String, LanguageData>>(json["translation"])!
            };
        }
    }
}
