using DevExpress.XtraPrinting.Native;
using RealtimeTable.Interfaces;
using RealtimeTable.Models;

namespace RealtimeTable.Services
{
    public class PushDataService : IPushDataService
    {
        private readonly IApiService _apiService;
        private const string Endpoint = "api/push";

        public PushDataService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<PushDataModel> GetStatus()
        {
            return await _apiService.Get<PushDataModel>(Endpoint + "/status");
        }

        public async Task<PushDataModel> Update(string type)
        {
            if (type == "start")
            {
                return await _apiService.PostUser<PushDataModel>(Endpoint + "/start", new object());
            }
            else if (type == "stop")
            {
                return await _apiService.PostUser<PushDataModel>(Endpoint + "/stop", new object());
            }
            else
            {
                return new PushDataModel();
            }
        }
    }
}
