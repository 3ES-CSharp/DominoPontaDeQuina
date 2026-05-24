using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Validador de jogadas, verifica se a peça pode ser colada no tabuleiro e se pertence ao jogador.
/// </summary>
public static class JogadaValidator
{
    /// <summary>
    /// Verifica se a jogada é válida no contexto do tabuleiro e da mão do jogador.
    /// </summary>
    /// <exception cref="JogadaInvalidaException">Lançada se a jogada for inválida.</exception>
    public static void Validar(Jogada jogada, Tabuleiro tabuleiro, MaoJogador maoJogador)
    {
        if (jogada.EhPassarVez())
            return; // Passar a vez é sempre válido

        if (jogada.Peca is null || jogada.Lado is null)
            throw new JogadaInvalidaException("Jogada inválida: peça ou lado não informado.");

        if (!maoJogador.ContemPeca(jogada.Peca.Value))
            throw new JogadaInvalidaException("Jogada inválida: a peça não pertence à mão do jogador.");

        if (!tabuleiro.PodeColar(jogada.Peca.Value, jogada.Lado.Value))
            throw new JogadaInvalidaException("Jogada inválida: a peça não é compatível com a ponta escolhida.");
    }
}