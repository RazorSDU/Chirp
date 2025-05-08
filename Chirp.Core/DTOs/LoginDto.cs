namespace Chirp.API.DTOs.Auth;

public record LoginDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}