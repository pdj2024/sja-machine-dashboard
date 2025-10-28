namespace RealtimeTable.Models
{
    public class TableColumn
    {
        public DateTime Timestamp { get; set; }
        //public Dictionary<string, double?> Tags { get; set; } = new();
        public Dictionary<string, string> Tags { get; set; } = new();
    }
}
