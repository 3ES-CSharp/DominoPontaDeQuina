using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Validators;

public static class JogadaValidator
{
    public static void Validar(Rodada rodada, Jogada jogada)
    {
        // Substituído ArgumentNullException por DominoException
        if (rodada == null) throw new DominoException("A rodada não pode ser nula.");
        if (jogada == null) throw new DominoException("A jogada não pode ser nula.");

        if (rodada.Status != Enums.StatusRodada.EmAndamento)
            throw new JogadaInvalidaException("A rodada não está em andamento.");

        // Comparação estrita pelo NOME do jogador para evitar conflitos de instâncias nos testes
        if (rodada.JogadorAtual != null && rodada.JogadorAtual.Jogador.Nome != jogada.Jogador.Nome)
            throw new JogadaInvalidaException($"Não é a vez do jogador {jogada.Jogador.Nome}.");

        if (!jogada.EhPassarVez())
        {
            if (!rodada.Tabuleiro.PodeColar(jogada.Peca!.Value, jogada.Lado!.Value))
                throw new JogadaInvalidaException("A peça não pode ser colada neste lado do tabuleiro.");
        }
    }
}