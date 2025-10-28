using RealtimeTable.Models;

namespace RealtimeTable.Interfaces
{
    public interface IPushDataService
    {
        Task<PushDataModel> GetStatus();
        Task<PushDataModel> Update(string type);
    }
}
