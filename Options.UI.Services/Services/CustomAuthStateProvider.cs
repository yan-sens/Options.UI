using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using OptionsStats.UI.Services.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Nodes;

namespace OptionsStats.UI.Services.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ISyncLocalStorageService _localStorage;
        public CustomAuthStateProvider(HttpClient httpClient, ISyncLocalStorageService localStorage)
        {
            this._httpClient = httpClient;
            this._localStorage = localStorage;

            var accessToken = _localStorage.GetItem<string>("accessToken");
            if (accessToken != null)
            {
                this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity()); // non-authonticated user

            try
            {
                var response = await _httpClient.GetAsync("api/account/profile");
                if (response.IsSuccessStatusCode)
                {
                    var strResponse = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    var email = jsonResponse!["email"]!.ToString();
                    var id = jsonResponse!["id"]!.ToString();

                    var claims = new List<Claim>
                    {
                        new("UserId", id),
                        new(ClaimTypes.Name, email),
                        new(ClaimTypes.Email, email)
                    };

                    // set the principal
                    var identity = new ClaimsIdentity(claims, "Token");
                    user = new ClaimsPrincipal(identity);

                    return new AuthenticationState(user);
                }
            }
            catch { }

            return new AuthenticationState(user);
        }

        public async Task<FormResult> LoginAsync(string email, string password)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("login", new { email, password });
                if (response.IsSuccessStatusCode)
                {
                    var strResponse = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    var accessToken = jsonResponse?["accessToken"]?.ToString();
                    var refreshToken = jsonResponse?["refreshToken"]?.ToString();

                    _localStorage.SetItem("accessToken", accessToken);
                    _localStorage.SetItem("refreshToken", refreshToken);

                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // need to refresh auth state
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

                    // success
                    return new FormResult { Succeded = true };
                }
                else
                {
                    return new FormResult { Succeded = false, Errors = ["Bad username or password"] };
                }
            }
            catch { }

            return new FormResult { Succeded = false, Errors = ["Connection Error"] };
        }

        public void Logout()
        {
            // delete tokens from localStorage
            _localStorage.RemoveItem("accessToken");
            _localStorage.RemoveItem("refreshToken");
            _httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<FormResult> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("register", registerDto);
                if(response.IsSuccessStatusCode)
                {
                    var loginResponse = await LoginAsync(registerDto.Email, registerDto.Password);
                    return loginResponse;
                }

                var strResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(strResponse);
                var jsonResponse = JsonNode.Parse(strResponse);
                var errorObjects = jsonResponse!["errors"]!.AsObject();
                var errorList = new List<string>();
                foreach (var error in errorObjects)
                {
                    errorList.Add(error.Value![0]!.ToString());
                }

                return new FormResult { Succeded = false, Errors = [.. errorList] };

            }
            catch { }

            return new FormResult { Succeded = false, Errors = ["Connection error"] };
        }
    }
}
