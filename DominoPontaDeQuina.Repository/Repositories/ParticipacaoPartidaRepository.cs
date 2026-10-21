using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class ParticipacaoPartidaRepository : IParticipacaoPartidaRepository
{
    private readonly DominoDbContext _context;

    public ParticipacaoPartidaRepository(DominoDbContext context)
    {
        _context = context;
    }

    public async Task<ParticipacaoPartida?> BuscarPorIdAsync(Guid id)
    {
        return await _context.ParticipacoesPartida
            .FirstOrDefaultAsync(pp => pp.Id == id);
    }

    public async Task<List<ParticipacaoPartida>> BuscarTodosAsync()
    {
        return await _context.ParticipacoesPartida
            .ToListAsync();
    }

    public async Task<List<ParticipacaoPartida>> BuscarPorPartidaAsync(Guid partidaId)
    {
        return await _context.ParticipacoesPartida
            .Where(pp => pp.PartidaId == partidaId)
            .ToListAsync();
    }

    public async Task<List<ParticipacaoPartida>> BuscarPorJogadorAsync(Guid jogadorId)
    {
        return await _context.ParticipacoesPartida
            .Where(pp => pp.JogadorId == jogadorId)
            .ToListAsync();
    }

    public async Task AdicionarAsync(ParticipacaoPartida participacao)
    {
        await _context.ParticipacoesPartida.AddAsync(participacao);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(ParticipacaoPartida participacao)
    {
        var existente = await _context.ParticipacoesPartida
            .FirstOrDefaultAsync(pp => pp.Id == participacao.Id);

        if (existente is null)
            return;

        existente.PartidaId = participacao.PartidaId;
        existente.JogadorId = participacao.JogadorId;
        existente.Posicao = participacao.Posicao;
        existente.Pontuacao = participacao.Pontuacao;
        existente.Vencedor = participacao.Vencedor;

        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(ParticipacaoPartida participacao)
    {
        _context.ParticipacoesPartida.Remove(participacao);
        await _context.SaveChangesAsync();
    }
}