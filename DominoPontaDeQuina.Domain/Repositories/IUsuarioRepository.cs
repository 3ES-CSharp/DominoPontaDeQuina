using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Domain.Repositories;

/// <summary>Define as operações de persistência de <see cref="Entities.Usuario"/>.</summary>
public interface IUsuarioRepository : IRepository<Usuario>
{
    /// <summary>Obtém uma conta de usuário pelo e-mail.</summary>
    /// <param name="email">E-mail usado na autenticação.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>O usuário ou <see langword="null"/> quando não encontrado.</returns>
    Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);
}
