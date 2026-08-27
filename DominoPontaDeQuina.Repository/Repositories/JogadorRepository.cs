using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class JogadorRepository : RepositoryBase<Jogador>
{
    public JogadorRepository(DominoDbContext context) : base(context)
    {
    }

    public List<Jogador> ListarPorUsuario(Guid usuarioId) =>
        Entidades.Where(j => j.UsuarioId == usuarioId)
                 .OrderBy(j => j.NomeExibicao)
                 .ToList();

    public Jogador? ObterPorNomeExibicao(string nomeExibicao) =>
        Entidades.Include(j => j.Usuario)
                 .FirstOrDefault(j => j.NomeExibicao == nomeExibicao);

    public int TotalDePartidas(Guid jogadorId) =>
        Entidades.Where(j => j.Id == jogadorId)
                 .SelectMany(j => j.Participacoes)
                 .Count();

    /// <summary>Ranking por vitorias, com desempate por pontos acumulados.</summary>
    public List<RankingJogador> Ranking(int quantidade = 10) =>
        Entidades.Select(j => new
                 {
                     j.Id,
                     j.NomeExibicao,
                     Vitorias = j.Participacoes.Count(p => p.Vencedor),
                     Pontos = j.Participacoes.Sum(p => (int?)p.Pontuacao) ?? 0
                 })
                 .OrderByDescending(r => r.Vitorias)
                 .ThenByDescending(r => r.Pontos)
                 .Take(quantidade)
                 .AsEnumerable()
                 .Select(r => new RankingJogador(r.Id, r.NomeExibicao, r.Vitorias, r.Pontos))
                 .ToList();
}

public record RankingJogador(Guid JogadorId, string NomeExibicao, int Vitorias, int Pontos);
