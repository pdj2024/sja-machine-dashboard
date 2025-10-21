using System.Text.Json.Serialization;

namespace Models.Machine
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
