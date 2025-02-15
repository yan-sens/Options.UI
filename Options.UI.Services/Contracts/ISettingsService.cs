using OptionsStats.UI.Services.Models;

namespace OptionsStats.UI.Services.Contracts
{
    public interface ISettingsService
    {
        Task<Settings?> GetSettingsAsync();
        Task<Settings> UpdateSettingsAsync(Settings userProfile);
    }
}
