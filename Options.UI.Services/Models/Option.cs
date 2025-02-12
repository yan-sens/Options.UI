using Options.UI.Services.Enums;

namespace Options.UI.Services.Models
{
    public class Option
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid? ParentOptionId { get; set; }

        public double Worth { get; set; }

        public double Price { get; set; }

        public double Premium { get; set; }

        public double Delta { get; set; }

        public int Contracts { get; set; }

        public bool IsClosed { get; set; }

        public bool Completed { get; set; }

        public double ReturnAmount { get; set; }

        public string TickerName { get; set; } = string.Empty;

        public OptionTypeEnum Type { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public ICollection<Option>? RollOvers { get; set; }
    }
}
