using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace PackageDelivery.UI.Api {
    public class ApiService : IApiService {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient client, IHttpContextAccessor httpContextAccessor) {
            this._client = client;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<T> Get<T>(string uri) where T : class {
            T result = null;
            
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _httpContextAccessor.HttpContext.Authentication.GetTokenAsync("access_token"));

            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode) {
                result = await response.Content.ReadAsAsync<T>();
            }

            return result;
        }

        public async Task<List<T>> GetMany<T>(string uri) where T : class
        {
            List<T> result = null;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                await _httpContextAccessor.HttpContext.Authentication.GetTokenAsync("access_token"));
            
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode) {
                result = await response.Content.ReadAsAsync<List<T>>();
            }

            return result;
        }

        public async Task<T> Post<T>(string uri, T value) where T : class {
            T result = null;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                await _httpContextAccessor.HttpContext.Authentication.GetTokenAsync("access_token"));

            var response = await _client.PostAsJsonAsync(uri, value);
            if (response.IsSuccessStatusCode) {
                result = await response.Content.ReadAsAsync<T>();
            }

            return result;
        }

        public async Task<T> Put<T>(string uri, T value) where T : class
        {
            T result = null;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                await _httpContextAccessor.HttpContext.Authentication.GetTokenAsync("access_token"));

            var response = await _client.PutAsJsonAsync(uri, value);
            if (response.IsSuccessStatusCode) {
                result = await response.Content.ReadAsAsync<T>();
            }

            return result;
        }

        public async Task<T> Delete<T>(string uri) where T : class
        {
            T result = null;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                await _httpContextAccessor.HttpContext.Authentication.GetTokenAsync("access_token"));

            var response = await _client.DeleteAsync(uri);
            if (response.IsSuccessStatusCode) {
                result = await response.Content.ReadAsAsync<T>();
            }

            return result;
        }
    }
}
