using System.ComponentModel.DataAnnotations;

public record LoginUserDto
{
    [Required]
    public string UserName { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;
} 