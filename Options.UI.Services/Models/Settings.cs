namespace OptionsStats.UI.Services.Models
{
    public class Settings
    {
        public required Guid Id { get; set; }

        public required Guid UserId { get; set; }

        public double RegulatoryFee { get; set; }

        public double Tax { get; set; }
    }
}
