using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AzureSpu221MyV.Data
{
    public class User
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("partitionKey")]
        public String PartitionKey { get; set; } = "users";
        [JsonProperty("name")]
        public String Name { get; set; } 
        [JsonProperty("email")]
        public String Email { get; set; }
        [JsonProperty("salt")]
        public String Salt { get; set; }
        [JsonProperty("dk")]
        public String DK { get; set; }
        [JsonProperty("birthdate")]
        public DateOnly Birthdate { get; set; }
        [JsonProperty("avatr")]
        public String? AvatarUrl { get; set; }

    }
}
