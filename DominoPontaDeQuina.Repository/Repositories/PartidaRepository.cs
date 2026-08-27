using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class PartidaRepository : RepositoryBase<Partida>
{
    public PartidaRepository(DominoDbContext context) : base(context)
    {
    }

    public List<Partida> ListarPorStatus(StatusPartida status) =>
        Entidades.Where(p => p.Status == status)
                 .OrderByDescending(p => p.IniciadoEm)
                 .ToList();

    public Partida? ObterComParticipacoes(Guid id) =>
        Entidades.Include(p => p.Participacoes)
                     .ThenInclude(x => x.Jogador)
                 .FirstOrDefault(p => p.Id == id);

    public List<Partida> ListarPorJogador(Guid jogadorId) =>
        Entidades.Where(p => p.Participacoes.Any(x => x.JogadorId == jogadorId))
                 .OrderByDescending(p => p.IniciadoEm)
                 .ToList();

    public List<Partida> ListarFinalizadasEntre(DateTime inicio, DateTime fim) =>
        Entidades.Where(p => p.FinalizadoEm != null
                          && p.FinalizadoEm >= inicio
                          && p.FinalizadoEm <= fim)
                 .OrderBy(p => p.FinalizadoEm)
                 .ToList();

    /// <summary>Partidas em aberto ha mais tempo do que o limite informado.</summary>
    public List<Partida> Travadas(TimeSpan limite)
    {
        var corte = DateTime.UtcNow - limite;
        return Entidades.Where(p => p.FinalizadoEm == null && p.IniciadoEm < corte)
                        .OrderBy(p => p.IniciadoEm)
                        .ToList();
    }
}
