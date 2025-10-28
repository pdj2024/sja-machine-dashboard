using System.Text.Json.Serialization;

namespace RealtimeTable.Models
{
    public class ApiUserResponse<T>
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public List<T> Data { get; set; }
    }
}
