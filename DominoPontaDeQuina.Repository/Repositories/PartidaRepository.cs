using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class PartidaRepository : IPartidaRepository
{
    private readonly DominoDbContext _context;

    public PartidaRepository(DominoDbContext context)
    {
        _context = context;
    }

    public async Task<Partida?> BuscarPorIdAsync(Guid id)
    {
        return await _context.Partidas
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Partida>> BuscarTodosAsync()
    {
        return await _context.Partidas
            .ToListAsync();
    }

    public async Task AdicionarAsync(Partida partida)
    {
        await _context.Partidas.AddAsync(partida);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Partida partida)
    {
        var existente = await _context.Partidas
            .FirstOrDefaultAsync(p => p.Id == partida.Id);

        if (existente is null)
            return;

        existente.IniciadoEm = partida.IniciadoEm;
        existente.FinalizadoEm = partida.FinalizadoEm;
        existente.Status = partida.Status;

        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Partida partida)
    {
        _context.Partidas.Remove(partida);
        await _context.SaveChangesAsync();
    }
}