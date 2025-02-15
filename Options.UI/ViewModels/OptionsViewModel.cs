using Microsoft.AspNetCore.Components;
using Options.UI.Services.Enums;
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

        public bool DeleteOptionDialogIsOpen { get; set; }

        public double RegulatoryFee { get; set; } = 0;

        public string StatsOptionWorth { get; set; } = string.Empty;

        public int StatsOptionCount { get; set; }

        public EventCallback<ICollection<Option>> OptionsChanged { get; set; }

        public OptionTypeEnum[] OptionTypes =
        [
            OptionTypeEnum.SellPut,
            OptionTypeEnum.SellCall
        ];

        public void CloseAllOptionDialogs()
        {
            CreateOptionDialogIsOpen = false;
            RollOverDialogIsOpen = false;
            CloseOptionDialogIsOpen = false;
            DeleteOptionDialogIsOpen = false;
            Option = new Option();
            ParentOptionId = null;
        }

        public void OpenRollOverDialog(Option currentOption)
        {
            var expirationDate = GetExpirationDate(currentOption);

            Option = new Option { StartDate = expirationDate, ParentOptionId = currentOption.Id };
            ParentOptionId = currentOption.Id;
            RollOverDialogIsOpen = true;
        }

        public void CalculateStatistics()
        {
            StatsOptionCount = OptionsList.Count;
            GetOptionsWorth();
        }

        public void GetOptionsWorth()
        {
            double total = 0;

            OptionsList.ToList().ForEach(option =>
            {
                var isOptionClosed = option.IsClosed || (option.RollOvers != null && option.RollOvers.Any(x => x.IsClosed));
                if(!isOptionClosed)
                {
                    total += option.Worth;
                }
            });

            StatsOptionWorth = "<span class='" + (total >= 0 ? "opt-green" : "opt-red") + "'>" + (total >= 0 ? "+" : "") + @String.Format("{0:C}", total) + "</span>";
        }

        public void OpenCloseOptionDialog(Option currentOption)
        {
            Option = currentOption;
            ParentOptionId = currentOption.Id;
            CloseOptionDialogIsOpen = true;
        }

        public void OpenDeleteOptionDialog(Option currentOption)
        {
            Option = currentOption;
            ParentOptionId = null;
            DeleteOptionDialogIsOpen = true;
        }

        public void OpenOptionDialog()
        {
            ParentOptionId = null;
            CreateOptionDialogIsOpen = true;
        }

        public string GetTotalFees(Option option)
        {
            var multiplieer = option.RollOvers != null ? option.RollOvers.Count + 1 : 1;
            var totalFees = RegulatoryFee * multiplieer;

            return @String.Format("{0:C}", totalFees);
        }

        public static string GetDuration(Option option)
        {
            var startDate = option.StartDate;
            var expirationDate = option.ExpirationDate;

            if (option.RollOvers != null)
            {
                option.RollOvers.ToList().ForEach(rollOver =>
                {
                    if (expirationDate < rollOver.ExpirationDate)
                    {
                        expirationDate = rollOver.ExpirationDate;
                    }
                });
            }

            var days = (startDate.Date - expirationDate.Date).Days;
            var response = -days + " days";

            return response;
        }

        public static string GetTotalDelta(Option option)
        {
            var deriver = option.RollOvers != null ? option.RollOvers.Count + 1 : 1;
            var totalDelta = option.Delta;
            if (option.RollOvers != null)
            {
                option.RollOvers.ToList().ForEach(rollOver => totalDelta += rollOver.Delta);
            }

            var result = totalDelta / deriver;

            return @String.Format("{0:0.00}", result);
        }

        public static string GetTotalPremium(Option option)
        {
            var total = option.Premium;
            if (option.RollOvers != null && option.RollOvers.Count > 0)
            {
                option.RollOvers.ToList().ForEach(x =>
                {
                    total += x.Premium;
                });
            }

            var response = "<span class='" + (total >= 0 ? "opt-green" : "opt-red") + "'>" + (total >= 0 ? "+" : "") + @String.Format("{0:C}", total) + "</span>";

            return response;
        }

        public static string GetRollOvelPremiumPercentage(Option rollOver, double worth)
        {
            var averagePremium = rollOver.Premium;
            var isNegatibe = averagePremium < 0;

            if (isNegatibe)
            {
                averagePremium = averagePremium * -1;
            }

            var totalPremiumProfit = (averagePremium / (worth / 100)) / 100;

            var response = "<span class='" + (!isNegatibe ? "opt-green" : "opt-red") + "'>" + (!isNegatibe ? "+" : "-") + totalPremiumProfit.ToString("P1") + "</span>";

            return response;
        }

        public static string GetTotalPremiumPercentage(Option option)
        {
            var averagePremium = option.Premium;
            if (option.RollOvers != null && option.RollOvers.Count > 0)
            {
                option.RollOvers.ToList().ForEach(x =>
                {
                    averagePremium += (int)x.Premium;
                });
            }

            var isNegatibe = averagePremium < 0;

            if (isNegatibe)
            {
                averagePremium = averagePremium * -1;
            }

            var totalPremiumProfit = (averagePremium / (option.Worth / 100)) / 100;

            var response = "<span class='" + (!isNegatibe ? "opt-green" : "opt-red") + "'>" + (!isNegatibe ? "+" : "") + totalPremiumProfit.ToString("P1") + "</span>";

            return response;
        }

        public static DateTime GetExpirationDate(Option option)
        {
            var expirationDate = option.ExpirationDate;

            if (option.RollOvers != null)
            {
                option.RollOvers.ToList().ForEach(rollOver =>
                {
                    if (expirationDate < rollOver.ExpirationDate)
                    {
                        expirationDate = rollOver.ExpirationDate;
                    }
                });
            }

            return expirationDate;
        }

        public static string GetExpirationDateString(Option option)
        {
            var expirationDate = GetExpirationDate(option);

            return expirationDate.ToString("dd/MM/yyyy");
        }

        public static string CalculateRollOverProfit(Option parent, Option rollOver)
        {
            var profit = (rollOver.Premium / (parent.Worth / 100)) / 100;
            var response = "<span class='" + (profit >= 0 ? "opt-green" : "opt-red") + "'>" + profit.ToString("P1") + "</span>";
            return response;
        }

        public static string GetOptionStatus(Option option)
        {
            var isClosed = option.IsClosed || (option.RollOvers != null && option.RollOvers!.Any(x => x.IsClosed));
            var response = "<span class='" + (!isClosed ? "opt-green" : "opt-red") + "'>" + (!isClosed ? "Open" : "Closed") + "</span>";
            return response;
        }

        public static string CalculateProfit(Option option)
        {
            var profit = (option.Premium / (option.Worth / 100)) / 100;
            var response = profit.ToString("P1");
            return response;
        }

    }
}
