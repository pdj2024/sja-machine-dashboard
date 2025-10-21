using Models.Machine;

namespace Interfaces
{
    public interface IMachineService
    {
        Task<Dictionary<string, List<ColumnMachine>>> Browse();
    }
}
