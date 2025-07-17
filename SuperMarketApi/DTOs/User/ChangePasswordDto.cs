using System.ComponentModel.DataAnnotations;

namespace SuperMarketApi.DTOs.User
{
    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;
        [Required]
        public string RepeatNewPassword { get; set; } = string.Empty;
    }
} 