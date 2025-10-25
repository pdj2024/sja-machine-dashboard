using System.Text.Json.Serialization;

namespace RealtimeTable.Models
{
    public class ColumnMachine
    {
        [JsonPropertyName("nama_tag")]
        public string NamaTag { get; set; }

        [JsonPropertyName("deskripsi")]
        public string Deskripsi { get; set; }

        [JsonPropertyName("alias_pendek")]
        public string AliasPendek { get; set; }
    }
}
