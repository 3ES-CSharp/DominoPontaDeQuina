using DominoPontaDeQuina.Core.Models;
using DominoPontaDeQuina.Core.Enums;
using System;

namespace DominoPontaDeQuina.Core.Services;

internal static class MaoJogadorService
{
    public static Jogada GetJogada(MaoJogador mao, Tabuleiro tabuleiro)
    {
        ArgumentNullException.ThrowIfNull(mao);
        ArgumentNullException.ThrowIfNull(tabuleiro);

        foreach (var lado in new[] { LadoTabuleiro.Direita, LadoTabuleiro.Esquerda })
        {
            foreach (var peca in mao.Pecas)
            {
                if (!TabuleiroService.PodeColar(tabuleiro, peca, lado))
                    continue;

                mao.RemoverPeca(peca);
                return new Jogada(mao.Jogador, peca, lado: lado);
            }
        }

        return new Jogada(mao.Jogador);
    }

    public static void DefazerJogada(MaoJogador mao, Jogada jogada)
    {
        ArgumentNullException.ThrowIfNull(mao);
        ArgumentNullException.ThrowIfNull(jogada);

        if (!jogada.EhPassarVez() && jogada.Peca is Peca peca)
            mao.AdicionarPecaInterno(peca);
    }
}
