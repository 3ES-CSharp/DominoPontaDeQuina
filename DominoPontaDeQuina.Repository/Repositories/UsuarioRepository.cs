using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <summary>
/// Repositório responsável pelas operações de persistência de <see cref="Usuario"/>.
/// </summary>
public class UsuarioRepository(DominoDbContext context)
{
    // ─────────────────────────────────────────────
    // Leitura
    // ─────────────────────────────────────────────

    /// <summary>Retorna um usuário pelo seu identificador único.</summary>
    public async Task<Usuario?> GetByIdAsync(Guid id)
        => await context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

    /// <summary>Retorna um usuário pelo endereço de e-mail.</summary>
    public async Task<Usuario?> GetByEmailAsync(string email)
        => await context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

    /// <summary>Retorna todos os usuários cadastrados.</summary>
    public async Task<List<Usuario>> GetAllAsync()
        => await context.Usuarios
            .AsNoTracking()
            .OrderBy(u => u.Nome)
            .ToListAsync();

    /// <summary>
    /// Retorna todos os usuários com seus jogadores associados (eager loading).
    /// </summary>
    public async Task<List<Usuario>> GetUsuariosComJogadoresAsync()
        => await context.Usuarios
            .AsNoTracking()
            .Include(u => u.Jogadores)
            .OrderBy(u => u.Nome)
            .ToListAsync();

    // ─────────────────────────────────────────────
    // Escrita
    // ─────────────────────────────────────────────

    /// <summary>Adiciona um novo usuário ao banco de dados.</summary>
    public async Task AddAsync(Usuario usuario)
    {
        await context.Usuarios.AddAsync(usuario);
        await context.SaveChangesAsync();
    }

    /// <summary>Atualiza os dados de um usuário existente.</summary>
    public async Task UpdateAsync(Usuario usuario)
    {
        context.Usuarios.Update(usuario);
        await context.SaveChangesAsync();
    }

    /// <summary>Remove um usuário pelo seu identificador único.</summary>
    public async Task DeleteAsync(Guid id)
    {
        var usuario = await context.Usuarios.FindAsync(id);
        if (usuario is not null)
        {
            context.Usuarios.Remove(usuario);
            await context.SaveChangesAsync();
        }
    }
}
