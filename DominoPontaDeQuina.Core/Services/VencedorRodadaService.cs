using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Servico responsavel por determinar o vencedor de uma rodada conforme o motivo de seu encerramento.
/// </summary>
/// <remarks>
/// Quando a rodada termina por batida, o vencedor e o jogador atual (aquele que ficou sem pecas na mao).
/// Quando termina por travamento do tabuleiro, o vencedor e aquele cuja mao apresenta a menor soma
/// dos valores das pecas restantes.
/// </remarks>
public static class VencedorRodadaService
{
    /// <summary>
    /// Define o vencedor da rodada com base no tipo de finalizacao e no estado das maos.
    /// </summary>
    /// <param name="fila">A fila de jogadores da rodada, em ordem de execucao.</param>
    /// <param name="tipoFinalizacao">O motivo do encerramento da rodada, quando houver.</param>
    /// <returns>O jogador vencedor, ou <see langword="null"/> quando a rodada ainda nao foi encerrada.</returns>
    public static Jogador? DefinirVencedor(Queue<MaoJogador> fila, TipoFinalizacaoRodada? tipoFinalizacao)
    {
        ArgumentNullException.ThrowIfNull(fila);

        if (tipoFinalizacao is null || fila.Count == 0)
            return null;

        return tipoFinalizacao switch
        {
            TipoFinalizacaoRodada.JogadorBateu => fila.Peek().Jogador,
            TipoFinalizacaoRodada.TabuleiroTravado => fila
                .OrderBy(mao => mao.SomarPecasNaMao())
                .First()
                .Jogador,
            _ => null
        };
    }
}
