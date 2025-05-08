namespace Chirp.API.DTOs.Auth;

public record AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
}