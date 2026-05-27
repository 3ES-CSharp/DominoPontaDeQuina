// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583) - Grupo 3ESPF/05
using System.Linq;
using DominoPontaDeQuina.Core.Models;
using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;

namespace DominoPontaDeQuina.Core.Validators;

/// <summary>
/// Validador das jogadas realizadas no dominó Ponta de Quina.
/// </summary>
public class JogadaValidator
{
    /// <summary>
    /// Valida se uma jogada é juridicamente aceita em relação ao estado atual do tabuleiro e à mão do jogador.
    /// </summary>
    /// <param name="jogada">A jogada a ser validada.</param>
    /// <param name="tabuleiro">O tabuleiro atual.</param>
    /// <param name="mao">A mão do jogador.</param>
    /// <returns>True se for válida; caso contrário, lança exceção do domínio.</returns>
    public bool Validar(Jogada jogada, Tabuleiro tabuleiro, MaoJogador mao)
    {
        if (jogada.EhPassarVez())
        {
            // Passar a vez só é válido se o jogador não tiver absolutamente nenhuma peça compatível com o tabuleiro
            if (tabuleiro.EstaVazio)
            {
                throw new JogadaInvalidaException("Não é permitido passar a vez com o tabuleiro vazio.");
            }

            // Verifica se o jogador tem alguma peça na mão que poderia ser colada
            bool temPecaCompativel = mao.Pecas.Any(peca => 
                tabuleiro.PodeColar(peca, LadoTabuleiro.Esquerda) || 
                tabuleiro.PodeColar(peca, LadoTabuleiro.Direita)
            );

            if (temPecaCompativel)
            {
                throw new JogadaInvalidaException("Não é permitido passar a vez quando há peças compatíveis na mão.");
            }

            return true;
        }

        // Se não for passar a vez, a peça e o lado devem ser especificados
        if (jogada.Peca is null || jogada.Lado is null)
        {
            throw new JogadaInvalidaException("A jogada deve especificar a peça e o lado a ser colado.");
        }

        // Valida se a peça pode de fato ser colada no lado escolhido do tabuleiro
        if (!tabuleiro.PodeColar(jogada.Peca.Value, jogada.Lado.Value))
        {
            throw new JogadaInvalidaException($"A peça {jogada.Peca.Value} não pode ser colada no lado {jogada.Lado.Value} do tabuleiro.");
        }

        return true;
    }
}
