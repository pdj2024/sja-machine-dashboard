namespace RealtimeTable.Models
{
    public class TableRealtime
    {
        public string Measurement { get; set; } = string.Empty;
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
