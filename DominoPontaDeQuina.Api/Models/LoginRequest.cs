namespace DominoPontaDeQuina.Api.Models;

/// <summary>Credenciais enviadas para autenticação.</summary>
/// <param name="Email">E-mail da conta.</param>
/// <param name="Senha">Senha da conta.</param>
public record LoginRequest(string Email, string Senha);
