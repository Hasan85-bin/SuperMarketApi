using System.ComponentModel.DataAnnotations;

public record CreateUserDto
{
    [Required]
    [StringLength(50, MinimumLength = 6)]
    public string UserName { get; init; } = string.Empty;
    
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; init; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; init; } = string.Empty;
    
    [Phone]
    public string? Phone { get; init; }
} 