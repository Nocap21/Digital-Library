using System.ComponentModel.DataAnnotations;

namespace DigitalLibrary.API.DTOs.Auth;

public class LoginRequest
{
    private string _username = string.Empty;

    [Required]
    public string Username { get => _username; set => _username = value?.Trim() ?? string.Empty; }

    [Required]
    public string Password { get; set; } = string.Empty;
}
