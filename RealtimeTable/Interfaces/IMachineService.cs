using RealtimeTable.Models;

namespace RealtimeTable.Interfaces
{
    public interface IMachineService
    {
        Task<Dictionary<string, List<ColumnMachine>>> Browse();
        Task<Dictionary<string, List<TableMachine>>> GetTableBy(object data, string type);
        Task<Dictionary<string, List<ColumnChart>>> GetChart(object data);
    }
}
