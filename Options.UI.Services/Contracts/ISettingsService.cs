using Options.UI.Services.Models;

namespace Options.UI.Services.Contracts
{
    public interface ISettingsService
    {
        Task<Settings?> GetSettingsByUserIdAsync(Guid userId);
        Task<Settings> UpdateSettingsAsync(Settings userProfile);
    }
}
