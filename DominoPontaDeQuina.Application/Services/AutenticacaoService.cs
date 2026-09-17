using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Domain.Repositories;

namespace DominoPontaDeQuina.Application.Services;

/// <summary>Implementa o caso de uso definido por <see cref="IAutenticacaoService"/>.</summary>
public sealed class AutenticacaoService(IUsuarioRepository usuarios) : IAutenticacaoService
{
    /// <inheritdoc />
    public async Task<Usuario> AutenticarAsync(string email, string senha, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O e-mail é obrigatório.", nameof(email));
        if (string.IsNullOrWhiteSpace(senha))
            throw new ArgumentException("A senha é obrigatória.", nameof(senha));

        var usuario = await usuarios.ObterPorEmailAsync(email, cancellationToken)
            ?? throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

        if (!HashDeSenha.Conferir(senha, usuario.SenhaHash))
            throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

        return usuario;
    }
}
