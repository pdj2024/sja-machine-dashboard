namespace RealtimeTable.Models
{
    public class TableRealtime
    {
        public string? TableName { get; set; }
        public string? Column { get; set; }
        public object? Value { get; set; }
        public string? Deskripsi { get; set; }
        public string? Unit { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
