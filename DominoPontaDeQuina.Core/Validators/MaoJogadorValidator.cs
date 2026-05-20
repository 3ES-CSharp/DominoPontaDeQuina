using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Validators;

/// <summary>
/// Concentra validacoes relacionadas ao estado da mao de um jogador.
/// Os metodos deste validator sao usados para decidir se uma jogada deve ser desfeita,
/// se a rodada terminou por batida ou para verificar a consistencia interna da mao.
/// </summary>
public static class MaoJogadorValidator
{
    /// <summary>
    /// Determina se a jogada informada deve ter sua peca devolvida a mao do jogador.
    /// Jogadas que representam apenas a passagem de vez nao envolvem peca e, portanto, nao precisam ser restauradas.
    /// </summary>
    /// <param name="jogada">A jogada que sera potencialmente desfeita.</param>
    /// <returns><see langword="true"/> quando ha peca para devolver a mao; caso contrario, <see langword="false"/>.</returns>
    public static bool DeveDevolverPeca(Jogada jogada)
    {
        ArgumentNullException.ThrowIfNull(jogada);

        return jogada.Peca is not null && !jogada.EhPassarVez();
    }

    /// <summary>
    /// Determina se a mao do jogador esta vazia.
    /// Uma mao vazia representa a condicao de batida e encerra a rodada com vitoria do jogador.
    /// </summary>
    /// <param name="mao">A mao avaliada.</param>
    /// <returns><see langword="true"/> quando a mao nao possui pecas; caso contrario, <see langword="false"/>.</returns>
    public static bool EstaSemPecas(MaoJogador mao)
    {
        ArgumentNullException.ThrowIfNull(mao);

        return mao.EstaSemPecas();
    }
}
