using System.Text.Json.Serialization;

namespace RealtimeTable.Models
{
    public class PushDataModel
    {
        [JsonPropertyName("running")]
        public bool Running { get; set; }

        [JsonPropertyName("lastPush")]
        public string? LastPush { get; set; }

        [JsonPropertyName("lastError")]
        public string? LastError { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
