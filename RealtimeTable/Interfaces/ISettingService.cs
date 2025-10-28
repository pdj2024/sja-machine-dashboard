using RealtimeTable.Models;

namespace RealtimeTable.Interfaces
{
    public interface ISettingService
    {
        Task<IEnumerable<Settings>> GetAll();
        Task<ApiSettingResponse<Settings>> Update(object setting);
    }
}
