using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;

namespace DominoPontaDeQuina.Core.Models;

/// <summary>
/// Representa o tabuleiro do jogo de dominó.
/// </summary>
public class Tabuleiro
{
    /// <summary>
    /// Lista de peças na ordem em que foram coladas.
    /// </summary>
    public List<Peca> Pecas { get; } = [];

    /// <summary>
    /// Indica se o tabuleiro está vazio.
    /// </summary>
    public bool EstaVazio => Pecas.Count == 0;

    /// <summary>
    /// Valor exposto na ponta esquerda.
    /// </summary>
    public int? PontaEsquerda => EstaVazio ? null : Pecas[0].ValorA;

    /// <summary>
    /// Valor exposto na ponta direita.
    /// </summary>
    public int? PontaDireita => EstaVazio ? null : Pecas[^1].ValorB;

    /// <summary>
    /// Verifica se uma peça pode ser colada em um determinado lado.
    /// </summary>
    public bool PodeColar(Peca peca, LadoTabuleiro lado)
    {
        if (EstaVazio) return true;
        int ponta = lado == LadoTabuleiro.Esquerda ? PontaEsquerda!.Value : PontaDireita!.Value;
        return peca.ValorA == ponta || peca.ValorB == ponta;
    }

    /// <summary>
    /// Cola uma peça no tabuleiro.
    /// </summary>
    public void Colar(Peca peca, LadoTabuleiro lado)
    {
        if (!PodeColar(peca, lado))
            throw new JogadaInvalidaException($"Não é possível colar a peça {peca}");

        Peca pecaParaColar = peca;

        if (!EstaVazio)
        {
            int ponta = lado == LadoTabuleiro.Esquerda ? PontaEsquerda!.Value : PontaDireita!.Value;
            
            // Inverte se o valor compatível está em ValorB
            if (peca.ValorB == ponta)
            {
                pecaParaColar = peca.Inverter();
            }
        }

        if (lado == LadoTabuleiro.Esquerda)
            Pecas.Insert(0, pecaParaColar);
        else
            Pecas.Add(pecaParaColar);
    }

    /// <summary>
    /// Soma os valores das pontas externas.
    /// </summary>
    public int SomarPontasExternas() =>
        EstaVazio ? 0 : PontaEsquerda!.Value + PontaDireita!.Value;

    /// <summary>
    /// Verifica se o tabuleiro está travado.
    /// </summary>
    public bool EstaTravado(IEnumerable<MaoJogador> maosJogadores)
    {
        if (EstaVazio) return false;
        foreach (var mao in maosJogadores)
            foreach (var peca in mao.ObterPecas())
                if (PodeColar(peca, LadoTabuleiro.Esquerda) || PodeColar(peca, LadoTabuleiro.Direita))
                    return false;
        return true;
    }

    /// <summary>
    /// Limpa o tabuleiro.
    /// </summary>
    public void Limpar() => Pecas.Clear();
}