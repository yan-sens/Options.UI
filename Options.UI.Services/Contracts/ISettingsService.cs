using Options.UI.Services.Models;

namespace Options.UI.Services.Contracts
{
    public interface ISettingsService
    {
        Task<Settings?> GetSettingsAsync();
        Task<Settings> UpdateSettingsAsync(Settings userProfile);
    }
}
