using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <summary>
/// Repositório responsável pelas operações de persistência de <see cref="ParticipacaoPartida"/>.
/// </summary>
public class ParticipacaoPartidaRepository(DominoDbContext context)
{
    // ─────────────────────────────────────────────
    // Leitura
    // ─────────────────────────────────────────────

    /// <summary>Retorna uma participação pelo seu identificador único.</summary>
    public async Task<ParticipacaoPartida?> GetByIdAsync(Guid id)
        => await context.ParticipacoesPartida
            .AsNoTracking()
            .Include(pp => pp.Jogador)
            .Include(pp => pp.Partida)
            .FirstOrDefaultAsync(pp => pp.Id == id);

    /// <summary>
    /// Retorna todas as participações de uma partida específica, ordenadas por posição.
    /// </summary>
    public async Task<List<ParticipacaoPartida>> GetByPartidaIdAsync(Guid partidaId)
        => await context.ParticipacoesPartida
            .AsNoTracking()
            .Where(pp => pp.PartidaId == partidaId)
            .Include(pp => pp.Jogador)
            .OrderBy(pp => pp.Posicao)
            .ToListAsync();

    /// <summary>
    /// Retorna todas as participações de um jogador específico, com as partidas associadas.
    /// </summary>
    public async Task<List<ParticipacaoPartida>> GetByJogadorIdAsync(Guid jogadorId)
        => await context.ParticipacoesPartida
            .AsNoTracking()
            .Where(pp => pp.JogadorId == jogadorId)
            .Include(pp => pp.Partida)
            .OrderByDescending(pp => pp.Partida.IniciadoEm)
            .ToListAsync();

    /// <summary>
    /// Retorna os vencedores de uma partida específica.
    /// </summary>
    public async Task<List<ParticipacaoPartida>> GetVencedoresDaPartidaAsync(Guid partidaId)
        => await context.ParticipacoesPartida
            .AsNoTracking()
            .Where(pp => pp.PartidaId == partidaId && pp.Vencedor)
            .Include(pp => pp.Jogador)
                .ThenInclude(j => j.Usuario)
            .ToListAsync();

    /// <summary>
    /// Retorna o ranking (classificação) de uma partida, ordenado por posição crescente.
    /// </summary>
    public async Task<List<ParticipacaoPartida>> GetRankingDaPartidaAsync(Guid partidaId)
        => await context.ParticipacoesPartida
            .AsNoTracking()
            .Where(pp => pp.PartidaId == partidaId)
            .Include(pp => pp.Jogador)
                .ThenInclude(j => j.Usuario)
            .OrderBy(pp => pp.Posicao)
            .ToListAsync();

    // ─────────────────────────────────────────────
    // Escrita
    // ─────────────────────────────────────────────

    /// <summary>Adiciona uma nova participação ao banco de dados.</summary>
    public async Task AddAsync(ParticipacaoPartida participacao)
    {
        await context.ParticipacoesPartida.AddAsync(participacao);
        await context.SaveChangesAsync();
    }

    /// <summary>Atualiza os dados de uma participação existente (ex: pontuação, vencedor).</summary>
    public async Task UpdateAsync(ParticipacaoPartida participacao)
    {
        context.ParticipacoesPartida.Update(participacao);
        await context.SaveChangesAsync();
    }

    /// <summary>Remove uma participação pelo seu identificador único.</summary>
    public async Task DeleteAsync(Guid id)
    {
        var participacao = await context.ParticipacoesPartida.FindAsync(id);
        if (participacao is not null)
        {
            context.ParticipacoesPartida.Remove(participacao);
            await context.SaveChangesAsync();
        }
    }
}
