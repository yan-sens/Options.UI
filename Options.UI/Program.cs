using BlazorBootstrap;
using Blazored.LocalStorage;
using MatBlazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OptionsStats.UI;
using OptionsStats.UI.Services.Contracts;
using OptionsStats.UI.Services.Services;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["RemovedWebApiAddress"]!) });

builder.Services.AddMatToaster(config =>
{
    config.Position = MatToastPosition.BottomRight;
    config.PreventDuplicates = true;
    config.NewestOnTop = true;
    config.ShowCloseButton = true;
    config.MaximumOpacity = 95;
    config.VisibleStateDuration = 3000;
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();
builder.Services.AddSingleton<PreloadService>();
builder.Services.AddMatBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddTransient<IOptionsService, OptionsService>();
builder.Services.AddTransient<ISettingsService, SettingsService>();
builder.Services.AddTransient<IUserProfileService, UserProfileService>();
builder.Services.AddBlazoredLocalStorage();

builder.Services
  .AddBlazorise()
  .AddBootstrap5Providers()
  .AddFontAwesomeIcons();

await builder.Build().RunAsync();
