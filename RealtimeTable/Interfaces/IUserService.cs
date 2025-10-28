using RealtimeTable.Models;

namespace RealtimeTable.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<ApiUserResponse<User>> Create(object user);
        Task<ApiUserResponse<User>> Update(string noId, object user);
        Task Delete(string noId);
    }
}
