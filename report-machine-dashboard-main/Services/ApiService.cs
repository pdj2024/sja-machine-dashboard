using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;
using Interfaces;
using Models;
using Microsoft.AspNetCore.Http;
using static System.Net.WebRequestMethods;

namespace Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        // Di masa depan, Anda bisa inject service untuk mengambil token di sini
        // private readonly ITokenProvider _tokenProvider;

        public ApiService(HttpClient httpClient /*, ITokenProvider tokenProvider */)
        {
            _httpClient = httpClient;
            // _tokenProvider = tokenProvider;
        }

        private async Task AddAuthHeader()
        {
            // --- PERSIAPAN UNTUK AUTENTIKASI ---
            // Di sini Anda akan mengambil token (misal JWT) dan menambahkannya ke header
            // var token = await _tokenProvider.GetTokenAsync();
            // if (!string.IsNullOrEmpty(token))
            // {
            //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // }

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> Get<T>(string baseUrl, Dictionary<string, string> queryParams = null)
        {
            await AddAuthHeader();

            var builder = new UriBuilder(_httpClient.BaseAddress.ToString() + baseUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            if (queryParams != null)
            {
                foreach (var param in queryParams)
                {
                    query[param.Key] = param.Value;
                }
            }
            
            builder.Query = query.ToString();
            string url = builder.ToString();

            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return default;
            }
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        
        public async Task<T> Post<T>(string baseUrl, object data)
        {
            await AddAuthHeader();
            
            var jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(baseUrl, jsonContent);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
