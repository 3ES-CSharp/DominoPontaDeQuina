using System.ComponentModel.DataAnnotations;

namespace DominoPontaDeQuina.WebApi.DTOs;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Senha { get; set; } = string.Empty;
}