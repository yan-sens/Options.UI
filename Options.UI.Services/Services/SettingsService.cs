using OptionsStats.UI.Services.Contracts;
using OptionsStats.UI.Services.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace OptionsStats.UI.Services.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly HttpClient _httpClient;

        public SettingsService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<Settings?> GetSettingsAsync()
        {
            return await _httpClient.GetFromJsonAsync<Settings>($"api/settings");
        }

        public async Task<Settings> UpdateSettingsAsync(Settings settings)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("api/settings", settings);
                if (response.IsSuccessStatusCode)
                {
                    var strResponse = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    return JsonSerializer.Deserialize<Settings>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? settings;
                }
            }
            catch { }

            return settings!;
        }
    }
}
