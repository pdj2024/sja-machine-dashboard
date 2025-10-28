using System.Text.Json.Serialization;

namespace RealtimeTable.Models
{
    public class Settings
    {
        [JsonPropertyName("realTimeFreq")]
        public int RealTimeFreq { get; set; }

        [JsonPropertyName("pushFreq")]
        public int PushFreq { get; set; }

        [JsonPropertyName("updatedAt")]
        public string UpdatedAt { get; set; }
    }
}
