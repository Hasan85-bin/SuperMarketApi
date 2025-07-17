using System.ComponentModel.DataAnnotations;

namespace SuperMarketApi.DTOs.User
{
    public class ChangeUserRoleDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty; // Accepts both string and int values
    }
}
