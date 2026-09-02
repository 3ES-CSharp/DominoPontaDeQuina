using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class JogadorRepository : IJogadorRepository
{
    private readonly DominoDbContext _context;

    public JogadorRepository(DominoDbContext context)
    {
        _context = context;
    }

    public async Task<Jogador?> BuscarPorIdAsync(Guid id)
    {
        return await _context.Jogadores
            .FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task<List<Jogador>> BuscarTodosAsync()
    {
        return await _context.Jogadores
            .ToListAsync();
    }

    public async Task<List<Jogador>> BuscarPorNomeAsync(string nome)
    {
        return await _context.Jogadores
            .Where(j => j.Nome.Contains(nome))
            .ToListAsync();
    }

    public async Task<List<Jogador>> BuscarPorUsuarioAsync(Guid usuarioId)
    {
        return await _context.Jogadores
            .Where(j => j.UsuarioId == usuarioId)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Jogador jogador)
    {
        await _context.Jogadores.AddAsync(jogador);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Jogador jogador)
    {
        _context.Jogadores.Update(jogador);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Jogador jogador)
    {
        _context.Jogadores.Remove(jogador);
        await _context.SaveChangesAsync();
    }
}