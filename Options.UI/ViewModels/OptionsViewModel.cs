using Options.UI.Services.Models;

namespace Options.UI.ViewModels
{
    public class OptionsViewModel
    {
        public Guid? ParentOptionId { get; set; }

        public Option Option { get; set; } = new Option();

        public ICollection<Option> OptionsList { get; set; } = [];

        public bool CreateOptionDialogIsOpen { get; set; }

        public bool RollOverDialogIsOpen { get; set; }

        public bool CloseOptionDialogIsOpen { get; set; }

        public double RegulatoryFee { get; set; } = 0;

        public void CloseAllOptionDialogs()
        {
            CreateOptionDialogIsOpen = false;
            RollOverDialogIsOpen = false;
            CloseOptionDialogIsOpen = false;
            Option = new Option();
            ParentOptionId = null;
        }
    }
}
