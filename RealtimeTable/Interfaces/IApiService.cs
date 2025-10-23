namespace RealtimeTable.Interfaces
{
    public interface IApiService
    {
        Task<T> Get<T>(string baseUrl, Dictionary<string, string> queryParams = null);
        Task<T> Post<T>(string baseUrl, object data, Dictionary<string, string> queryParams = null);

        Task<T> PostUser<T>(string baseUrl, object data);
        Task<T> Put<T>(string baseUrl, string id, object data);
        Task Delete(string baseUrl, string id);
    }
}
