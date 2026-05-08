using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Serviço responsável por validar se uma jogada é permitida pelas regras do jogo.
/// </summary>
internal class JogadaValidator : IJogadaValidator
{
    /// <summary>
    /// Valida se uma jogada específica é permitida no estado atual do tabuleiro.
    /// </summary>
    public bool ValidarJogada(Jogada jogada, Tabuleiro tabuleiro, MaoJogador maoJogador)
    {
        if (jogada.EhPassarVez())
            return !PossuiPecaCompativel(maoJogador, tabuleiro);

        if (jogada.Peca == null || jogada.Lado == null)
            return false;

        if (!maoJogador.PossuiPeca(jogada.Peca.Value))
            return false;

        return tabuleiro.PodeColar(jogada.Peca.Value, jogada.Lado.Value);
    }

    /// <summary>
    /// Verifica se o jogador possui alguma peça compatível com o estado atual do tabuleiro.
    /// </summary>
    public bool PossuiPecaCompativel(MaoJogador maoJogador, Tabuleiro tabuleiro)
    {
        if (tabuleiro.EstaVazio)
            return maoJogador.QuantidadePecas > 0;

        foreach (var peca in maoJogador.ObterPecas())
            if (tabuleiro.PodeColar(peca, LadoTabuleiro.Esquerda) ||
                tabuleiro.PodeColar(peca, LadoTabuleiro.Direita))
                return true;
        return false;
    }
}