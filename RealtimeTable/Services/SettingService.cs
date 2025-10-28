using RealtimeTable.Interfaces;
using RealtimeTable.Models;

namespace RealtimeTable.Services
{
    public class SettingService : ISettingService
    {
        private readonly IApiService _apiService;
        private const string Endpoint = "api/settings";

        public SettingService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IEnumerable<Settings>> GetAll()
        {
            var queryParams = new Dictionary<string, string> {};

            var response = await _apiService.Get<ApiSettingResponse<Settings>>(Endpoint + "/getAll", queryParams);

            if (response != null && response.StatusCode == 200)
            {
                return new List<Settings> { response.Data };
            }

            return Enumerable.Empty<Settings>();
        }

        public async Task<ApiSettingResponse<Settings>> Update(object setting)
        {
            return await _apiService.PutSetting<ApiSettingResponse<Settings>>(Endpoint + "/update", setting);
        }
    }
}
