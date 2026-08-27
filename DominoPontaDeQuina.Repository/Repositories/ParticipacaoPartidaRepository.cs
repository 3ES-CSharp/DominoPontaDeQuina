using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class ParticipacaoPartidaRepository : RepositoryBase<ParticipacaoPartida>
{
    public ParticipacaoPartidaRepository(DominoDbContext context) : base(context)
    {
    }

    public List<ParticipacaoPartida> ListarPorPartida(Guid partidaId) =>
        Entidades.Include(p => p.Jogador)
                 .Where(p => p.PartidaId == partidaId)
                 .OrderBy(p => p.Posicao)
                 .ToList();

    public ParticipacaoPartida? ObterVencedor(Guid partidaId) =>
        Entidades.Include(p => p.Jogador)
                 .FirstOrDefault(p => p.PartidaId == partidaId && p.Vencedor);

    public List<ParticipacaoPartida> ListarPorJogador(Guid jogadorId) =>
        Entidades.Include(p => p.Partida)
                 .Where(p => p.JogadorId == jogadorId)
                 .OrderByDescending(p => p.Partida.IniciadoEm)
                 .ToList();

    public int TotalDePontos(Guid jogadorId) =>
        Entidades.Where(p => p.JogadorId == jogadorId)
                 .Sum(p => (int?)p.Pontuacao) ?? 0;

    /// <summary>Media de pontos por posicao final, para analise de equilibrio do jogo.</summary>
    public List<(int Posicao, double MediaPontos)> MediaDePontosPorPosicao() =>
        Entidades.GroupBy(p => p.Posicao)
                 .Select(g => new { g.Key, Media = g.Average(p => p.Pontuacao) })
                 .OrderBy(x => x.Key)
                 .AsEnumerable()
                 .Select(x => (x.Key, x.Media))
                 .ToList();
}
