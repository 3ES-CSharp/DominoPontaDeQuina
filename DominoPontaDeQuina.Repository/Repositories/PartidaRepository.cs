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

    /// <summary>Retorna todas as partidas com o status informado.</summary>
    public async Task<List<Partida>> GetByStatusAsync(StatusPartida status)
        => await context.Partidas
            .AsNoTracking()
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.IniciadoEm)
            .ToListAsync();

    /// <summary>Retorna todas as partidas com status <see cref="StatusPartida.EmAndamento"/>.</summary>
    public Task<List<Partida>> GetPartidasEmAndamentoAsync()
        => GetByStatusAsync(StatusPartida.EmAndamento);

    /// <summary>Retorna o histórico de partidas das quais um jogador participou.</summary>
    public async Task<List<Partida>> GetHistoricoPorJogadorAsync(Guid jogadorId)
        => await context.Partidas
            .AsNoTracking()
            .Where(p => p.Participacoes.Any(pp => pp.JogadorId == jogadorId))
            .OrderByDescending(p => p.IniciadoEm)
            .ToListAsync();

    /// <summary>Retorna uma partida com todos os seus participantes e jogadores associados.</summary>
    public async Task<Partida?> GetComParticipantesAsync(Guid id)
        => await context.Partidas
            .AsNoTracking()
            .Include(p => p.Participacoes)
                .ThenInclude(pp => pp.Jogador)
            .FirstOrDefaultAsync(p => p.Id == id);

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
