using Interfaces;
using Models.Machine;

namespace Services
{
    public class MachineService : IMachineService
    {
        private readonly IApiService _apiService;
        private const string Endpoint = "api/tag";

        public MachineService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<Dictionary<string, List<ColumnMachine>>> Browse()
        {
            var response = await _apiService.Get<ApiResponse<Dictionary<string, List<ColumnMachine>>>>(Endpoint + "/browse");

            if (response?.Data != null)
            {
                return response.Data;
            }

            return new Dictionary<string, List<ColumnMachine>>();
        }
    }
}
