using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <summary>
/// Repositório responsável pelas operações de persistência de <see cref="Jogador"/>.
/// </summary>
public class JogadorRepository(DominoDbContext context)
{
    // ─────────────────────────────────────────────
    // Leitura
    // ─────────────────────────────────────────────

    /// <summary>Retorna um jogador pelo seu identificador único.</summary>
    public async Task<Jogador?> GetByIdAsync(Guid id)
        => await context.Jogadores
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.Id == id);

    /// <summary>Retorna todos os jogadores cadastrados, ordenados por nome de exibição.</summary>
    public async Task<List<Jogador>> GetAllAsync()
        => await context.Jogadores
            .AsNoTracking()
            .OrderBy(j => j.NomeExibicao)
            .ToListAsync();

    /// <summary>Retorna todos os jogadores vinculados a um usuário específico.</summary>
    public async Task<List<Jogador>> GetByUsuarioIdAsync(Guid usuarioId)
        => await context.Jogadores
            .AsNoTracking()
            .Where(j => j.UsuarioId == usuarioId)
            .OrderBy(j => j.NomeExibicao)
            .ToListAsync();

    /// <summary>
    /// Retorna todos os jogadores com suas participações e as partidas relacionadas (eager loading).
    /// </summary>
    public async Task<List<Jogador>> GetJogadoresComParticipacaoAsync()
        => await context.Jogadores
            .AsNoTracking()
            .Include(j => j.Participacoes)
                .ThenInclude(p => p.Partida)
            .OrderBy(j => j.NomeExibicao)
            .ToListAsync();

    // ─────────────────────────────────────────────
    // Escrita
    // ─────────────────────────────────────────────

    /// <summary>Adiciona um novo jogador ao banco de dados.</summary>
    public async Task AddAsync(Jogador jogador)
    {
        await context.Jogadores.AddAsync(jogador);
        await context.SaveChangesAsync();
    }

    /// <summary>Atualiza os dados de um jogador existente.</summary>
    public async Task UpdateAsync(Jogador jogador)
    {
        context.Jogadores.Update(jogador);
        await context.SaveChangesAsync();
    }

    /// <summary>Remove um jogador pelo seu identificador único.</summary>
    public async Task DeleteAsync(Guid id)
    {
        var jogador = await context.Jogadores.FindAsync(id);
        if (jogador is not null)
        {
            context.Jogadores.Remove(jogador);
            await context.SaveChangesAsync();
        }
    }
}
