using System.Collections.Generic;
using System.Linq;
using DominoPontaDeQuina.Core.Enums;

namespace DominoPontaDeQuina.Core.Models;

public class Tabuleiro
{
    private readonly LinkedList<Peca> _pecas = new();

    public bool EstaVazio => !_pecas.Any();

    public int PontaEsquerda => _pecas.First!.Value.ValorA;

    public int PontaDireita => _pecas.Last!.Value.ValorB;

    public bool PodeColar(Peca peca, LadoTabuleiro lado)
    {
        if (EstaVazio)
            return true;

        return lado switch
        {
            LadoTabuleiro.Esquerda => peca.PossuiValor(PontaEsquerda),
            LadoTabuleiro.Direita => peca.PossuiValor(PontaDireita),
            _ => false
        };
    }

    public void Colar(Peca peca, LadoTabuleiro lado)
    {
        if (!PodeColar(peca, lado))
            throw new JogadaInvalidaException("Peca invalida.");

        if (EstaVazio)
        {
            _pecas.AddFirst(peca);
            return;
        }

        if (lado == LadoTabuleiro.Esquerda)
        {
            _pecas.AddFirst(
                peca.ValorB == PontaEsquerda ? peca : peca.Inverter());
        }
        else
        {
            _pecas.AddLast(
                peca.ValorA == PontaDireita ? peca : peca.Inverter());
        }
    }

    public bool EstaTravado(IEnumerable<MaoJogador> maos)
    {
        if (EstaVazio)
            return false;

        return !maos.Any(m =>
            m.Pecas.Any(p =>
                PodeColar(p, LadoTabuleiro.Esquerda) ||
                PodeColar(p, LadoTabuleiro.Direita)));
    }

    public int SomarPontasExternas()
    {
        if (EstaVazio)
            return 0;

        if (_pecas.Count == 1)
            return _pecas.First!.Value.SomaValores;

        return PontaEsquerda + PontaDireita;
    }
}