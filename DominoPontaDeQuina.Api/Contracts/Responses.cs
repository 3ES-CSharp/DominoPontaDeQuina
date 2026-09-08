using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Api.Contracts;

/// <summary>Representação de uma partida na API.</summary>
/// <param name="Id">Identificador da partida.</param>
/// <param name="PontuacaoAlvo">Pontuação necessária para vencer.</param>
/// <param name="Status">Status atual da partida.</param>
/// <param name="CriadaEm">Data de criação em UTC.</param>
public sealed record PartidaResponse(Guid Id, int PontuacaoAlvo, string Status, DateTime CriadaEm)
{
    /// <summary>Cria a resposta a partir da entidade de domínio.</summary>
    /// <param name="partida">Entidade de origem.</param>
    /// <returns>DTO correspondente.</returns>
    public static PartidaResponse De(Partida partida) =>
        new(partida.Id, partida.PontuacaoAlvo, partida.Status, partida.CriadaEm);
}

/// <summary>Representação de um jogador na API.</summary>
/// <param name="Id">Identificador do jogador.</param>
/// <param name="Nome">Nome exibido.</param>
public sealed record JogadorResponse(Guid Id, string Nome)
{
    /// <summary>Cria a resposta a partir da entidade de domínio.</summary>
    /// <param name="jogador">Entidade de origem.</param>
    /// <returns>DTO correspondente.</returns>
    public static JogadorResponse De(Jogador jogador) => new(jogador.Id, jogador.Nome);
}

/// <summary>Representação de um lance na API.</summary>
/// <param name="Id">Identificador do lance.</param>
/// <param name="Timestamp">Instante do lance em UTC.</param>
public sealed record LanceResponse(Guid Id, DateTime Timestamp)
{
    /// <summary>Cria a resposta a partir da entidade de domínio.</summary>
    /// <param name="lance">Entidade de origem.</param>
    /// <returns>DTO correspondente.</returns>
    public static LanceResponse De(Lance lance) => new(lance.Id, lance.Timestamp);
}

/// <summary>Linha do ranking de vitórias.</summary>
/// <param name="Jogador">Nome do jogador classificado.</param>
/// <param name="Vitorias">Total de vitórias.</param>
public sealed record RankingResponse(string Jogador, int Vitorias)
{
    /// <summary>Cria a resposta a partir da entidade de domínio.</summary>
    /// <param name="ranking">Entidade de origem.</param>
    /// <returns>DTO correspondente.</returns>
    public static RankingResponse De(Ranking ranking) => new(ranking.Jogador?.Nome ?? string.Empty, ranking.Vitorias);
}
