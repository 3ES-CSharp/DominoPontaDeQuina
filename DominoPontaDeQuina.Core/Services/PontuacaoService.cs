using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Servico responsavel pela regra de pontuacao da jogada e pelo creditamento de pontos ao time correspondente.
/// </summary>
/// <remarks>
/// A regra do dominio estabelece que a soma das pontas externas do tabuleiro, quando multipla de 5,
/// gera pontos para o time do jogador que efetuou a jogada. O total de pontos da jogada e <c>soma / 5</c>.
/// </remarks>
public static class PontuacaoService
{
    /// <summary>
    /// Multiplicador usado para validar e calcular a pontuacao a partir da soma das pontas externas.
    /// </summary>
    private const int MultiploPontuacao = 5;

    /// <summary>
    /// Calcula a pontuacao da jogada com base na soma das pontas externas do tabuleiro.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro consultado.</param>
    /// <returns>
    /// O numero de pontos gerados pela jogada quando a soma das pontas e multipla de <see cref="MultiploPontuacao"/>;
    /// caso contrario, zero.
    /// </returns>
    /// <exception cref="ArgumentNullException">Lancada quando <paramref name="tabuleiro"/> e <see langword="null"/>.</exception>
    public static int CalcularPontosDaJogada(Tabuleiro tabuleiro)
    {
        ArgumentNullException.ThrowIfNull(tabuleiro);

        if (tabuleiro.EstaVazio)
            return 0;

        var soma = tabuleiro.SomarPontasExternas();
        if (soma == 0 || soma % MultiploPontuacao != 0)
            return 0;

        return soma / MultiploPontuacao;
    }

    /// <summary>
    /// Credita os pontos ao time do jogador informado, dentre os times participantes da partida.
    /// </summary>
    /// <param name="times">A colecao de times da partida.</param>
    /// <param name="jogador">O jogador autor da jogada que gerou os pontos.</param>
    /// <param name="pontos">A quantidade de pontos a creditar. Quando menor ou igual a zero, a operacao e ignorada.</param>
    /// <returns><see langword="true"/> quando os pontos forem creditados; caso contrario, <see langword="false"/>.</returns>
    public static bool CreditarPontosAoTime(IEnumerable<Time>? times, Jogador? jogador, int pontos)
    {
        if (times is null || jogador is null || pontos <= 0)
            return false;

        var time = times.FirstOrDefault(t => t.PossuiJogador(jogador));
        if (time is null)
            return false;

        time.SomarPontos(pontos);
        return true;
    }
}
