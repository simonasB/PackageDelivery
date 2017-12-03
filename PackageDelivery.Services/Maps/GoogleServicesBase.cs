using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PackageDelivery.Services.Maps {
    public abstract class GoogleServicesBase {
        protected readonly HttpClient _httpClient;

        protected const string APIKey = "AIzaSyBI4Wp8uObgOkkHoBd2k4K6s8VEiY3zeKc";

        protected const string BaseUri = "https://maps.googleapis.com/maps/api";

        protected GoogleServicesBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected T ExecuteRequest<T>(Uri uri) where T : class, new()
        {
            var response = _httpClient.GetAsync(uri).Result;

            T result = default(T);

            if (response.IsSuccessStatusCode)
            {
                var resultAsString = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<T>(resultAsString, new StringEnumConverter());
            }

            return result;
        }

        protected string ExecuteRequest(Uri uri)
        {
            var response = _httpClient.GetAsync(uri).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }

            return "";
        }
    }
}
