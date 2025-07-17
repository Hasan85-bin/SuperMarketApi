using System.ComponentModel.DataAnnotations;

public record UpdateUserInfoDto
{
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; init; } = string.Empty;

    [Phone]
    public string? Phone { get; init; }
} 