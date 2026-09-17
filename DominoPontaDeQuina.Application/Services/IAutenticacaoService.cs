using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Application.Services;

/// <summary>Expõe o caso de uso de autenticação de usuários.</summary>
public interface IAutenticacaoService
{
    /// <summary>Autentica uma conta a partir das credenciais informadas.</summary>
    /// <param name="email">E-mail da conta.</param>
    /// <param name="senha">Senha da conta em texto puro.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>O usuário autenticado.</returns>
    Task<Usuario> AutenticarAsync(string email, string senha, CancellationToken cancellationToken = default);
}
