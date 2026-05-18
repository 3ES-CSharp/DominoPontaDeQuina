using DominoPontaDeQuina.Core.Models;
using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace DominoPontaDeQuina.Core.Services;

internal static class TabuleiroService
{
    public static bool PodeColar(Tabuleiro tabuleiro, Peca peca, LadoTabuleiro lado)
    {
        if (tabuleiro.EstaVazio)
            return true;

        return lado switch
        {
            LadoTabuleiro.Esquerda => peca.PossuiValor(tabuleiro.PontaEsquerda!.Value),
            LadoTabuleiro.Direita => peca.PossuiValor(tabuleiro.PontaDireita!.Value),
            _ => false
        };
    }

    public static void Colar(Tabuleiro tabuleiro, Peca peca, LadoTabuleiro lado)
    {
        if (!PodeColar(tabuleiro, peca, lado))
            throw new DominoPontaDeQuinaException("A peça não pode ser colada no lado informado.");

        if (tabuleiro.EstaVazio)
        {
            tabuleiro.Pecas.Add(peca);
            return;
        }

        var pecaPosicionada = lado == LadoTabuleiro.Esquerda
            ? peca.ValorA == tabuleiro.PontaEsquerda!.Value
                ? peca.Inverter()
                : peca
            : peca.ValorB == tabuleiro.PontaDireita!.Value
                ? peca.Inverter()
                : peca;

        if (lado == LadoTabuleiro.Esquerda)
            tabuleiro.Pecas.Insert(0, pecaPosicionada);
        else
            tabuleiro.Pecas.Add(pecaPosicionada);
    }

    public static bool EstaTravado(Tabuleiro tabuleiro, IEnumerable<MaoJogador> maosJogadores)
    {
        if (tabuleiro.EstaVazio)
            return false;

        return maosJogadores.All(mao =>
            mao.Pecas.All(peca => !PodeColar(tabuleiro, peca, LadoTabuleiro.Esquerda) && !PodeColar(tabuleiro, peca, LadoTabuleiro.Direita)));
    }
}
