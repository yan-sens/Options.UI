using Options.UI.Services.Contracts;
using Options.UI.Services.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Options.UI.Services.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly HttpClient _httpClient;

        public UserProfileService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<UserProfile?> GetUserProfileByIdAsync(Guid userId)
        {
            return await _httpClient.GetFromJsonAsync<UserProfile>($"api/account/profile");
        }

        public async Task<UserProfile> UpdateUserProfileAsync(UserProfile userProfile)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("api/account", userProfile);
                if (response.IsSuccessStatusCode)
                {
                    var strResponse = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    return JsonSerializer.Deserialize<UserProfile>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? userProfile;
                }
            }
            catch { }

            return userProfile!;
        }

        public async Task<UserProfile> CreateUserProfileAsync(UserProfile userProfile)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/account", userProfile);
                if (response.IsSuccessStatusCode)
                {
                    var strResponse = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    return JsonSerializer.Deserialize<UserProfile>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? userProfile;
                }
            }
            catch { }

            return userProfile!;
        }

        public async Task DeleteUserProfileAsync(Guid userId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/account?userId={userId}");
                if (response.IsSuccessStatusCode)
                {

                }
            }
            catch { }
        }
    }
}
