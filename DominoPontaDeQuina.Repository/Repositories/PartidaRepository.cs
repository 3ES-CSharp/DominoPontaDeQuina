using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <summary>
/// Repositório responsável pelas operações de persistência de <see cref="Partida"/>.
/// </summary>
public class PartidaRepository(DominoDbContext context)
{
    // ─────────────────────────────────────────────
    // Leitura
    // ─────────────────────────────────────────────

    /// <summary>Retorna uma partida pelo seu identificador único.</summary>
    public async Task<Partida?> GetByIdAsync(Guid id)
        => await context.Partidas
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

    /// <summary>Retorna todas as partidas cadastradas, ordenadas pela data de início.</summary>
    public async Task<List<Partida>> GetAllAsync()
        => await context.Partidas
            .AsNoTracking()
            .OrderByDescending(p => p.IniciadoEm)
            .ToListAsync();

    /// <summary>Retorna todas as partidas com status <see cref="StatusPartida.EmAndamento"/>.</summary>
    public async Task<List<Partida>> GetPartidasEmAndamentoAsync()
        => await context.Partidas
            .AsNoTracking()
            .Where(p => p.Status == StatusPartida.EmAndamento)
            .OrderByDescending(p => p.IniciadoEm)
            .ToListAsync();

    /// <summary>Retorna todas as partidas com status <see cref="StatusPartida.Finalizado"/>.</summary>
    public async Task<List<Partida>> GetPartidasFinalizadasAsync()
        => await context.Partidas
            .AsNoTracking()
            .Where(p => p.Status == StatusPartida.Finalizado)
            .OrderByDescending(p => p.FinalizadoEm)
            .ToListAsync();

    /// <summary>
    /// Retorna uma partida com todos os seus participantes, jogadores e usuários associados.
    /// </summary>
    public async Task<Partida?> GetPartidasComParticipantesAsync(Guid id)
        => await context.Partidas
            .AsNoTracking()
            .Include(p => p.Participacoes)
                .ThenInclude(pp => pp.Jogador)
                    .ThenInclude(j => j.Usuario)
            .FirstOrDefaultAsync(p => p.Id == id);

    /// <summary>
    /// Retorna todas as partidas com seus participantes (eager loading completo).
    /// </summary>
    public async Task<List<Partida>> GetTodasPartidasComParticipantesAsync()
        => await context.Partidas
            .AsNoTracking()
            .Include(p => p.Participacoes)
                .ThenInclude(pp => pp.Jogador)
                    .ThenInclude(j => j.Usuario)
            .OrderByDescending(p => p.IniciadoEm)
            .ToListAsync();

    // ─────────────────────────────────────────────
    // Escrita
    // ─────────────────────────────────────────────

    /// <summary>Adiciona uma nova partida ao banco de dados.</summary>
    public async Task AddAsync(Partida partida)
    {
        await context.Partidas.AddAsync(partida);
        await context.SaveChangesAsync();
    }

    /// <summary>Atualiza os dados de uma partida existente.</summary>
    public async Task UpdateAsync(Partida partida)
    {
        context.Partidas.Update(partida);
        await context.SaveChangesAsync();
    }

    /// <summary>Remove uma partida pelo seu identificador único.</summary>
    public async Task DeleteAsync(Guid id)
    {
        var partida = await context.Partidas.FindAsync(id);
        if (partida is not null)
        {
            context.Partidas.Remove(partida);
            await context.SaveChangesAsync();
        }
    }
}
