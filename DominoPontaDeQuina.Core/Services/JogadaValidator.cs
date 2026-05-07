using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Serviço responsável por validar se uma jogada é permitida pelas regras do jogo.
/// </summary>
internal class JogadaValidator : IJogadaValidator
{
    public bool ValidarJogada(Jogada jogada, Tabuleiro tabuleiro, MaoJogador maoJogador)
    {
        // CASO 1: Jogador está passando a vez → só pode se não tiver peças compatíveis
        if (jogada.EhPassarVez())
            return !PossuiPecaCompativel(maoJogador, tabuleiro);

        // CASO 2: Dados inconsistentes
        if (jogada.Peca == null || jogada.Lado == null)
            return false;

        // CASO 3: Jogador não possui a peça
        if (!maoJogador.PossuiPeca(jogada.Peca.Value))
            return false;

        // CASO 4: Verifica compatibilidade com o tabuleiro
        return tabuleiro.PodeColar(jogada.Peca.Value, jogada.Lado.Value);
    }

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