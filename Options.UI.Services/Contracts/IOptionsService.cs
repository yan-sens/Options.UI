using OptionsStats.UI.Services.Models;

namespace OptionsStats.UI.Services.Contracts
{
    public interface IOptionsService
    {
        Task<List<Option>> GetOptionsAsync(OptionsFilter filter);
        Task<Option> CreateOptionAsync(Option option);
        Task<Option> UpdateOptionAsync(Option option);
        Task DeleteOptionAsync(Guid optionId);
    }
}
