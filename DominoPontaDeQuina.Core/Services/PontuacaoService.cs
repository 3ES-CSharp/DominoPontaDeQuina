using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Calcula e atribui pontuação ao time do jogador com base no estado do tabuleiro após uma jogada.
/// A regra de pontuação concede pontos quando a soma das pontas externas for múltiplo de 5.
/// </summary>
public static class PontuacaoService
{
    /// <summary>
    /// Atribui pontos ao time do jogador quando a soma das pontas externas do tabuleiro
    /// for um múltiplo de 5. A quantidade de pontos é igual à soma dividida por 5.
    /// </summary>
    /// <param name="jogador">O jogador que realizou a jogada.</param>
    /// <param name="tabuleiro">O tabuleiro com o estado atual após a jogada.</param>
    /// <param name="times">Os times participantes da partida.</param>
    public static void AtribuirPontos(Jogador jogador, Tabuleiro tabuleiro, IEnumerable<Time> times)
    {
        var soma = tabuleiro.SomarPontasExternas();

        if (soma <= 0 || soma % 5 != 0)
            return;

        int pontos = soma / 5;
        var time = times.FirstOrDefault(t => t.PossuiJogador(jogador));
        time?.SomarPontos(pontos);
    }
}
