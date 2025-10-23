using System.Text.Json.Serialization;

namespace RealtimeTable.Models
{
    public class User
    {
        [JsonPropertyName("no_id")]
        public string No_Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
