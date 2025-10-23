using RealtimeTable.Interfaces;
using RealtimeTable.Models;

namespace RealtimeTable.Services
{
    public class UserService : IUserService
    {
        private readonly IApiService _apiService;
        private const string Endpoint = "api/user";

        public UserService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var queryParams = new Dictionary<string, string> {};

            var response = await _apiService.Get<ApiUserResponse<User>>(Endpoint + "/getAll", queryParams);

            if (response != null && response.StatusCode == 200)
            {
                return response.Data;
            }

            return Enumerable.Empty<User>();
        }

        public async Task<ApiUserResponse<User>> Create(object user)
        {
            return await _apiService.PostUser<ApiUserResponse<User>>(Endpoint + "/create", user);
        }

        public async Task<ApiUserResponse<User>> Update(string noId, object user)
        {
            return await _apiService.Put<ApiUserResponse<User>>(Endpoint + "/update", noId, user);
        }

        public async Task Delete(string noId)
        {
            await _apiService.Delete(Endpoint + "/delete", noId);
        }
    }
}
