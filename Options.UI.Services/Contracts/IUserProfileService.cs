using Options.UI.Services.Models;

namespace Options.UI.Services.Contracts
{
    public interface IUserProfileService
    {
        Task<UserProfile?> GetUserProfileByIdAsync(Guid userId);
        Task<UserProfile> CreateUserProfileAsync(UserProfile userProfile);
        Task<UserProfile> UpdateUserProfileAsync(UserProfile userProfile);
        Task DeleteUserProfileAsync(Guid userId);
    }
}
