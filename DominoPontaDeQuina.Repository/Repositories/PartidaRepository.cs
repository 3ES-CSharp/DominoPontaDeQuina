using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class PartidaRepository
{
    private readonly DominoDbContext _context;

    public PartidaRepository(DominoDbContext context)
    {
        _context = context;
    }

    public async Task<Partida?> BuscarPorIdAsync(Guid id)
    {
        return await _context.Partidas
            .Include(p => p.Participacoes)
            .ThenInclude(pp => pp.Jogador)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Partida>> BuscarTodosAsync()
    {
        return await _context.Partidas
            .Include(p => p.Participacoes)
            .ToListAsync();
    }

    public async Task<List<Partida>> BuscarPorStatusAsync(StatusJogo status)
    {
        return await _context.Partidas
            .Where(p => p.Status == status)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Partida partida)
    {
        await _context.Partidas.AddAsync(partida);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Partida partida)
    {
        _context.Partidas.Update(partida);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Partida partida)
    {
        _context.Partidas.Remove(partida);
        await _context.SaveChangesAsync();
    }
}