using Options.UI.Services.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Options.UI
{
    public static class OptionValidator
    {
        public static ValidationResponse IsValid(this Option option)
        {
            var response = new ValidationResponse();

            if (option.ParentOptionId == null && string.IsNullOrEmpty(option.TickerName))
            {
                response.ErrorMessage = "Please enter a ticker name.";
                return response;
            }
            else if (option.ParentOptionId == null && option.Contracts < 1)
            {
                response.ErrorMessage = "Shold be at least 1 contract.";
                return response;
            }
            else if (option.StartDate == DateTime.MinValue)
            {
                response.ErrorMessage = "Pease enter an option start date.";
                return response;
            }
            else if (option.ExpirationDate == DateTime.MinValue)
            {
                response.ErrorMessage = "Pease enter an option expiration date.";
                return response;
            }
            else if (option.StartDate == option.ExpirationDate)
            {
                response.ErrorMessage = $"Expiration date should be more then {option.StartDate.Date:dd/MM/yyyy}.";
                return response;
            }
            else if (option.ExpirationDate < option.StartDate)
            {
                response.ErrorMessage = "Expiration date can't be less then a start date.";
                return response;
            }
            else if (option.Delta < 0 || option.Delta > 1)
            {
                response.ErrorMessage = "Delta should be in range 0 to 1";
                return response;
            }
            else if (option.ParentOptionId == null && option.Worth <= 0)
            {
                response.ErrorMessage = "Worth should be positive.";
                return response;
            }
            else if (option.ParentOptionId == null && option.Worth < option.Premium)
            {
                response.ErrorMessage = "Worth can't be more then premium.";
                return response;
            }
            else if (option.Price <= 0)
            {
                response.ErrorMessage = "Price should be positive.";
                return response;
            }

            response.IsValid = true;

            return response;
        }

        public static ValidationResponse ValidateClosingOption(this Option option, DateTime expirationDate)
        {
            var response = new ValidationResponse();

            if (option.IsClosed)
            {
                response.ErrorMessage = string.Format($"Option is already closed.");
                return response;
            }

            if (option.Completed)
            {
                if (option.ReturnAmount < 0)
                {
                    response.ErrorMessage = string.Format($"Return amound should be positive.");
                    return response;
                }
            }

            if (!option.ClosedDate.HasValue || option.ClosedDate == DateTime.MinValue)
            {
                response.ErrorMessage = string.Format($"Closed date is requeired.");
                return response;
            }

            var startDate = option.StartDate;
            option.RollOvers?.ToList().ForEach(r =>
            {
                if (startDate < r.StartDate)
                {
                    startDate = r.StartDate;
                }
            });
            if (option.ClosedDate.Value.Date <= startDate.Date)
            {
                response.ErrorMessage = $"Closed date should be more then {startDate.Date:dd/MM/yyyy}.";
                return response;
            }

            if (option.ClosedDate.Value.Date > expirationDate.Date)
            {
                response.ErrorMessage = $"Closed date should be more then {expirationDate.Date:dd/MM/yyyy}.";
                return response;
            }


            response.IsValid = true;
            return response;
        }

        public static ValidationResponse ValidateRollOver(this Option option, Option rollOver)
        {
            var response = new ValidationResponse();
            var expirationDate = option.ExpirationDate;
            var startDate = option.StartDate;

            option.RollOvers?.ToList().ForEach(r =>
                {
                    if (startDate < r.StartDate)
                    {
                        startDate = r.StartDate;
                    }
                    if (expirationDate < r.ExpirationDate)
                    {
                        expirationDate = r.ExpirationDate;
                    }
                });

            if (rollOver.StartDate.Date <= startDate.Date)
            {
                response.ErrorMessage = string.Format($"Start date should be more then {startDate:dd/MM/yyyy}.");
                return response;
            }

            if (rollOver.StartDate.Date > expirationDate.Date)
            {
                response.ErrorMessage = string.Format($"Start date can't be more then {expirationDate:dd/MM/yyyy}.");
                return response;
            }

            else if (rollOver.ExpirationDate.Date <= expirationDate.Date)
            {
                response.ErrorMessage = $"Expiration date should be more then {expirationDate.Date}.";
                return response;
            }

            response.IsValid = true;

            return response;

        }
    }

    public class ValidationResponse
    {
        public bool IsValid { get; set; } = false;

        public string ErrorMessage { get; set; } = string.Empty;
    }
}
