using System.Text.Json.Serialization;

namespace Api.Models
{
    public class User
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }
        
        [JsonPropertyName("login")]
        public string Login { get; set; }
    }
}