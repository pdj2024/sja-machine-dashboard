using System.Text.Json;
using RealtimeTable.Interfaces;
using RealtimeTable.Models;

namespace RealtimeTable.Services
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

            return response?.Data ?? new Dictionary<string, List<ColumnMachine>>();
        }

        public async Task<Dictionary<string, List<TableMachine>>> GetTableBy(object data, string type, bool latestData = false)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "request_by", type },
                { "latest_data_only", Convert.ToString(latestData) }
            };

            var response = await _apiService.Post<ApiTableResponse>(Endpoint + "/table", data, queryParams);
            var result = new Dictionary<string, List<TableMachine>>();

            if (response == null || response.Data.ValueKind != JsonValueKind.Array)
                return result;

            foreach (var element in response.Data.EnumerateArray())
            {
                var timestamp = element.GetProperty("timestamp").GetString();

                foreach (var property in element.EnumerateObject())
                {
                    if (property.Name.Equals("timestamp", StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (!result.ContainsKey(property.Name))
                        result[property.Name] = new List<TableMachine>();

                    double value = 0;
                    if (property.Value.ValueKind == JsonValueKind.Number)
                        value = property.Value.GetDouble();

                    result[property.Name].Add(new TableMachine
                    {
                        Timestamp = timestamp,
                        Value = value
                    });
                }
            }

            return result;
        }

        public async Task<Dictionary<string, List<ColumnChart>>> GetChart(object data)
        {
            var response = await _apiService.Post<ApiResponse<Dictionary<string, List<ColumnChart>>>>(Endpoint + "/chart", data);

            return response?.Data ?? new Dictionary<string, List<ColumnChart>>();
        }
    }
}
