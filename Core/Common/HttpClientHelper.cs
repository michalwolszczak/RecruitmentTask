using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace Core.Common
{
    public class HttpClientHelper
    {
        private readonly HttpClient _httpClient;

        public HttpClientHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> SendRequestAsync<T>(string endpoint, object requestBody, HttpMethod method, Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = GetRequestUri(endpoint),
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"),                
            };

            AddHeaders(request, headers);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>() 
                ?? throw new ArgumentNullException(nameof(T));
        }

        private void AddHeaders(HttpRequestMessage request, Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
        }
        private Uri GetRequestUri(string endpointName)
        {
            return new Uri(_httpClient.BaseAddress, endpointName);
        }
    }
}