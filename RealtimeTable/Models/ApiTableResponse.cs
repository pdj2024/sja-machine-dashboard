using System.Text.Json;
using System.Text.Json.Serialization;

namespace RealtimeTable.Models
{
    public class ApiTableResponse
    {
        [JsonPropertyName("data")]
        public JsonElement Data { get; set; }
    }
}
