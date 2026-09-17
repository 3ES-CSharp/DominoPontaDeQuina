using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Infrastructure.Persistence.Repositories;

/// <summary>Implementa a persistência de usuários com o <see cref="DominoDbContext"/>.</summary>
public sealed class UsuarioRepository : EfRepository<Usuario>, IUsuarioRepository
{
    readonly DominoDbContext db;

    /// <summary>Inicializa o repositório de usuários.</summary>
    public UsuarioRepository(DominoDbContext db) : base(db) => this.db = db;
    /// <inheritdoc />
    public async Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await db.Usuarios.AsNoTracking().SingleOrDefaultAsync(usuario => usuario.Email == email, cancellationToken);
    }
}
