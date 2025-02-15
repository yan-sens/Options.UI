﻿
namespace OptionsStats.UI.Services.Models
{
    public class UserProfile
    {
        public required Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
