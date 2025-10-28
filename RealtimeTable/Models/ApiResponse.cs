using System.Text.Json.Serialization;

namespace RealtimeTable.Models
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
