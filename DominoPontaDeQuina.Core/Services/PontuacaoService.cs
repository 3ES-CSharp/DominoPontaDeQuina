using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Servico responsavel por consolidar pontuacoes de partida e aplicar regras de pontuacao da rodada.
/// Centraliza as operacoes aritmeticas e de validacao das regras de quina utilizadas pelo dominio.
/// </summary>
public static class PontuacaoService
{
    /// <summary>
    /// Divisor utilizado pela regra do dominio ponta de quina para conceder pontos.
    /// Pontos sao atribuidos quando a soma das pontas externas for multiplo deste valor.
    /// </summary>
    public const int DivisorPontosQuina = 5;

    /// <summary>
    /// Calcula a pontuacao acumulada de cada time da partida.
    /// </summary>
    /// <param name="times">Os times participantes da partida.</param>
    /// <returns>Um dicionario com a pontuacao acumulada de cada time.</returns>
    public static Dictionary<Time, int> ObterPontuacaoTimes(IEnumerable<Time> times)
    {
        ArgumentNullException.ThrowIfNull(times);

        var pontuacaoTimes = new Dictionary<Time, int>();

        foreach (var time in times)
        {
            pontuacaoTimes.Add(time, time.Pontuacao);
        }

        return pontuacaoTimes;
    }

    /// <summary>
    /// Identifica o primeiro time que atingiu ou ultrapassou a pontuacao alvo.
    /// </summary>
    /// <param name="times">Os times participantes da partida.</param>
    /// <param name="pontuacaoAlvo">A pontuacao alvo da partida.</param>
    /// <returns>O time vencedor, ou <see langword="null"/> quando ainda nao houver vencedor.</returns>
    public static Time? ObterTimeVencedor(IEnumerable<Time> times, int pontuacaoAlvo)
    {
        ArgumentNullException.ThrowIfNull(times);

        foreach (var time in times)
        {
            if (time.Pontuacao >= pontuacaoAlvo)
                return time;
        }

        return null;
    }

    /// <summary>
    /// Determina se a configuracao atual da rodada e elegivel para conceder pontos.
    /// A rodada concede pontos somente quando ja possui um <see cref="TipoFinalizacaoRodada"/> definido
    /// e quando a soma das pontas externas do tabuleiro e multiplo de <see cref="DivisorPontosQuina"/>.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro avaliado.</param>
    /// <param name="tipoFinalizacao">O tipo de finalizacao corrente da rodada, quando houver.</param>
    /// <returns><see langword="true"/> quando a rodada e elegivel para pontuacao; caso contrario, <see langword="false"/>.</returns>
    public static bool DeveAtribuirPontos(Tabuleiro tabuleiro, TipoFinalizacaoRodada? tipoFinalizacao)
    {
        ArgumentNullException.ThrowIfNull(tabuleiro);

        if (tipoFinalizacao is null)
            return false;

        var soma = tabuleiro.SomarPontasExternas();
        return soma % DivisorPontosQuina == 0;
    }
}
