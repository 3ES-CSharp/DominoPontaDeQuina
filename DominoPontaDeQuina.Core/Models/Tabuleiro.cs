using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;

namespace DominoPontaDeQuina.Core.Models;

/// <summary>
/// Representa o tabuleiro no nivel da rodada dentro da hierarquia Partida -> Rodadas -> Jogadas.
/// </summary>
public class Tabuleiro
{
    /// <summary>
    /// Lista de peças na ordem em que foram coladas. Índice 0 = ponta esquerda.
    /// </summary>
    public List<Peca> Pecas { get; } = [];

    /// <summary>
    /// Indica se o tabuleiro ainda não possui peças coladas.
    /// </summary>
    public bool EstaVazio => Pecas.Count == 0;

    /// <summary>
    /// Valor exposto na ponta esquerda (ValorA da primeira peça).
    /// </summary>
    public int? PontaEsquerda => EstaVazio ? null : Pecas[0].ValorA;

    /// <summary>
    /// Valor exposto na ponta direita (ValorB da última peça).
    /// </summary>
    public int? PontaDireita => EstaVazio ? null : Pecas[^1].ValorB;

    /// <summary>
    /// Verifica se uma peça pode ser colada em um determinado lado do tabuleiro.
    /// </summary>
    public bool PodeColar(Peca peca, LadoTabuleiro lado)
    {
        if (EstaVazio) return true;
        int ponta = lado == LadoTabuleiro.Esquerda ? PontaEsquerda!.Value : PontaDireita!.Value;
        return peca.PossuiValor(ponta);
    }

    /// <summary>
    /// Cola uma peça no tabuleiro no lado especificado.
    /// </summary>
    public void Colar(Peca peca, LadoTabuleiro lado)
    {
        if (!PodeColar(peca, lado))
            throw new JogadaInvalidaException($"Não é possível colar a peça {peca} no lado {lado}.");

        Peca pecaParaColar = peca;
        if (!EstaVazio)
        {
            int ponta = lado == LadoTabuleiro.Esquerda ? PontaEsquerda!.Value : PontaDireita!.Value;
            if (peca.ValorB == ponta && peca.ValorA != ponta)
                pecaParaColar = peca.Inverter();
        }

        if (lado == LadoTabuleiro.Esquerda)
            Pecas.Insert(0, pecaParaColar);
        else
            Pecas.Add(pecaParaColar);
    }

    /// <summary>
    /// Soma os valores atuais das pontas externas do tabuleiro.
    /// </summary>
    public int SomarPontasExternas() =>
        EstaVazio ? 0 : PontaEsquerda!.Value + PontaDireita!.Value;

    /// <summary>
    /// Verifica se o tabuleiro está travado (nenhum jogador tem peças compatíveis).
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
    /// Limpa o tabuleiro para uma nova rodada.
    /// </summary>
    public void Limpar() => Pecas.Clear();
}