using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Pcf.GivingToCustomer.Core.Abstractions.Gateways;

namespace Pcf.GivingToCustomer.Integration
{
    public class DictionaryGateway<T>(HttpClient httpClient) : IDictionaryGateway<T>
        where T : class
    {
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var response = await httpClient.GetAsync($"api/v1/Dictionaries/{typeof(T).Name}");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseBody)) return null;

            var result = JsonSerializer.Deserialize<IEnumerable<T>>(responseBody, _options);

            return result;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var response = await httpClient.GetAsync($"api/v1/Dictionaries/{typeof(T).Name}/{id}");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseBody)) return null;

            var result = JsonSerializer.Deserialize<T>(responseBody, _options);

            return result;
        }
    }
}
