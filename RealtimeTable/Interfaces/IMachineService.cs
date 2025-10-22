using RealtimeTable.Models;

namespace RealtimeTable.Interfaces
{
    public interface IMachineService
    {
        Task<Dictionary<string, List<ColumnMachine>>> Browse();
        //Task<List<Dictionary<string, object>>> GetTableBy(object data, string type, bool latestData = false);

        Task<Dictionary<string, List<TableMachine>>> GetTableBy(object data, string type, bool latestData = false);
        Task<Dictionary<string, List<ColumnChart>>> GetChart(object data);
    }
}
