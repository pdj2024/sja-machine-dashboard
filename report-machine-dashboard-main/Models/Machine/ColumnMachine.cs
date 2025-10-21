using System.Text.Json.Serialization;

namespace Models.Machine
{
    public class ColumnMachine
    {
        [JsonPropertyName("nama_tag")]
        public string NamaTag { get; set; }

        [JsonPropertyName("deskripsi")]
        public string Deskripsi { get; set; }
    }
}
