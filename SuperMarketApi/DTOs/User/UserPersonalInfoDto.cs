namespace SuperMarketApi.DTOs.User
{
    public class UserPersonalInfoDto
    {
        public int ID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Role { get; set; } = string.Empty;
    }
} 