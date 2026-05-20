using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Servico responsavel por selecionar o primeiro jogador de uma rodada com base na hierarquia
/// de criterios do dominio: posse da sena, posse da maior carroca e, por fim, maior soma de pecas.
/// </summary>
public static class SelecaoJogadorService
{
    /// <summary>
    /// Seleciona o primeiro jogador da rodada considerando os criterios padrao do domino ponta de quina.
    /// </summary>
    /// <remarks>
    /// Ordem de prioridade aplicada:
    /// <list type="number">
    ///   <item><description>Jogador que possui a peca sena [6|6].</description></item>
    ///   <item><description>Jogador que possui a maior carroca disponivel (de [5|5] ate [0|0]).</description></item>
    ///   <item><description>Jogador com a maior soma de valores na mao, como criterio de desempate final.</description></item>
    /// </list>
    /// </remarks>
    /// <param name="maosJogadores">As maos dos jogadores participantes da rodada.</param>
    /// <returns>O jogador escolhido para iniciar a rodada.</returns>
    /// <exception cref="RodadaInvalidaException">
    /// Lancada quando a colecao de maos esta vazia, pois nao ha jogadores possiveis para iniciar a rodada.
    /// </exception>
    public static Jogador SelecionarPrimeiroJogador(IList<MaoJogador> maosJogadores)
    {
        ArgumentNullException.ThrowIfNull(maosJogadores);

        if (maosJogadores.Count == 0)
            throw new RodadaInvalidaException("Nao ha jogadores disponiveis para iniciar a rodada.");

        var jogadorComSena = ObterJogadorComSena(maosJogadores);
        if (jogadorComSena is not null)
            return jogadorComSena;

        var jogadorComCarroca = ObterJogadorComMaiorCarroca(maosJogadores);
        if (jogadorComCarroca is not null)
            return jogadorComCarroca;

        return ObterJogadorComMaiorSoma(maosJogadores);
    }

    /// <summary>
    /// Procura o primeiro jogador que possui a peca sena ([6|6]).
    /// </summary>
    /// <param name="maosJogadores">As maos avaliadas.</param>
    /// <returns>O jogador detentor da sena, ou <see langword="null"/> quando ninguem a possui.</returns>
    private static Jogador? ObterJogadorComSena(IEnumerable<MaoJogador> maosJogadores)
    {
        foreach (var mao in maosJogadores)
        {
            if (mao.PossuiSena())
            {
                return mao.Jogador;
            }
        }

        return null;
    }

    /// <summary>
    /// Procura o jogador detentor da maior carroca disponivel, varrendo de [5|5] ate [0|0].
    /// </summary>
    /// <param name="maosJogadores">As maos avaliadas.</param>
    /// <returns>O jogador detentor da maior carroca, ou <see langword="null"/> quando ninguem possui carrocas.</returns>
    private static Jogador? ObterJogadorComMaiorCarroca(IEnumerable<MaoJogador> maosJogadores)
    {
        for (int i = 5; i >= 0; i--)
        {
            foreach (var mao in maosJogadores)
            {
                if (mao.Pecas.Any(p => p.ValorA == i || p.ValorA == i))
                {
                    return mao.Jogador;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Identifica o jogador cuja soma das pecas na mao e a maior, como criterio final de desempate.
    /// </summary>
    /// <param name="maosJogadores">As maos avaliadas.</param>
    /// <returns>O jogador com a maior soma de valores na mao.</returns>
    private static Jogador ObterJogadorComMaiorSoma(IEnumerable<MaoJogador> maosJogadores)
    {
        int maior = 0;
        Jogador? jogadorMaior = null;

        foreach (var mao in maosJogadores)
        {
            var soma = mao.SomarPecasNaMao();
            if (soma > maior)
            {
                maior = soma;
                jogadorMaior = mao.Jogador;
            }
        }

        return jogadorMaior
            ?? throw new RodadaInvalidaException("Nao foi possivel determinar o primeiro jogador da rodada.");
    }
}
