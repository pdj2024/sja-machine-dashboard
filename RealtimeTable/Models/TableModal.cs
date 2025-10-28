namespace RealtimeTable.Models
{
    public class TableModal
    {
        public string Name { get; set; }
        public List<ColumnModal> Columns { get; set; } = new();
    }
}
