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

    public void Adicionar(Partida partida)
    {
        _context.Partidas.Add(partida);
        _context.SaveChanges();
    }

    public List<Partida> Listar()
    {
        return _context.Partidas
            .Include(partida => partida.Participacoes)
            .ToList();
    }

    public Partida? BuscarPorId(Guid id)
    {
        return _context.Partidas
            .Include(partida => partida.Participacoes)
            .FirstOrDefault(partida => partida.Id == id);
    }

    public List<Partida> BuscarPorStatus(StatusJogo status)
    {
        return _context.Partidas
            .Where(partida => partida.Status == status)
            .ToList();
    }

    public void Atualizar(Partida partida)
    {
        _context.Partidas.Update(partida);
        _context.SaveChanges();
    }

    public void Excluir(Guid id)
    {
        var partida = _context.Partidas
            .FirstOrDefault(partida => partida.Id == id);

        if (partida != null)
        {
            _context.Partidas.Remove(partida);
            _context.SaveChanges();
        }
    }
}