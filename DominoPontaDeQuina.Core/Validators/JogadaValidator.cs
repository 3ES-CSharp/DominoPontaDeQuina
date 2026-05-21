using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Validators;

/// <summary>
/// Classe responsável por validar as regras de negócio de uma jogada.
/// </summary>
public static class JogadaValidator
{
    /// <summary>
    /// Valida se a jogada fornecida pode ser executada na rodada atual.
    /// </summary>
    /// <param name="rodada">A rodada que está em andamento.</param>
    /// <param name="jogada">A tentativa de jogada efetuada pelo jogador.</param>
    public static void Validar(Rodada rodada, Jogada jogada)
    {
        if (rodada == null) throw new DominoException("A rodada não pode ser nula.");
        if (jogada == null) throw new DominoException("A jogada não pode ser nula.");

        if (rodada.Status != Enums.StatusRodada.EmAndamento)
            throw new JogadaInvalidaException("A rodada não está em andamento.");

        if (rodada.JogadorAtual != null && rodada.JogadorAtual.Jogador.Nome != jogada.Jogador.Nome)
            throw new JogadaInvalidaException($"Não é a vez do jogador {jogada.Jogador.Nome}.");

        if (!jogada.EhPassarVez())
        {
            if (!rodada.Tabuleiro.PodeColar(jogada.Peca!.Value, jogada.Lado!.Value))
                throw new JogadaInvalidaException("A peça não pode ser colada neste lado do tabuleiro.");
        }
    }
}