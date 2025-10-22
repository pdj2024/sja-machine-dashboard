using System.Text.Json.Serialization;

namespace RealtimeTable.Models
{
    public class ColumnChart
    {
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("value")]
        public double Value { get; set; }
    }
}
