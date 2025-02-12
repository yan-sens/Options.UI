using Options.UI.Services.Models;

namespace Options.UI.Services.Contracts
{
    public interface IOptionsService
    {
        Task<List<Option>> GetOptionsAsync(OptionsFilter filter);
        Task<Option> CreateOptionAsync(Option option);
        Task<Option> UpdateOptionAsync(Option option);
    }
}
