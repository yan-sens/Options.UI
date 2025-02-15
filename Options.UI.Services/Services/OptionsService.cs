using OptionsStats.UI.Services.Contracts;
using OptionsStats.UI.Services.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace OptionsStats.UI.Services.Services
{
    public class OptionsService : IOptionsService
    {
        private readonly HttpClient _httpClient;

        public OptionsService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<List<Option>> GetOptionsAsync(OptionsFilter filter)
        {
            var options = new List<Option>();
            JsonSerializerOptions serializerOptions = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNameCaseInsensitive = true
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/options/filter", filter);
                if (response.IsSuccessStatusCode)
                {
                    var strResponse = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);

                    options = JsonSerializer.Deserialize<List<Option>>(jsonResponse, serializerOptions);
                }
            }
            catch { }
            
            return options!;
        }

        public async Task<Option> UpdateOptionAsync(Option option)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("api/options", option);
                if (response.IsSuccessStatusCode)
                {
                    var strResponse = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    return JsonSerializer.Deserialize<Option>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? option;
                }
            }
            catch { }

            return option!;
        }

        public async Task<Option> CreateOptionAsync(Option option)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/options", option);
                if (response.IsSuccessStatusCode)
                {
                    var strResponse = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    return JsonSerializer.Deserialize<Option>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? option;
                }
            }
            catch { }

            return option!;
        }

        public async Task DeleteOptionAsync(Guid optionId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/options/{optionId}");
                if (response.IsSuccessStatusCode)
                {

                }
            }
            catch { }
        }
    }
}
