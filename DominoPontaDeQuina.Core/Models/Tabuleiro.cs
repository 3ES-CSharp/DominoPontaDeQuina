using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;

namespace DominoPontaDeQuina.Core.Models;

// CLASSE MODIFICADA PELO ALUNO - GRUPO 01
// Gaps implementados: PodeColar(), Colar(), EstaTravado()
public class Tabuleiro
{
    public List<Peca> Pecas { get; } = [];
    public bool EstaVazio => Pecas.Count == 0;
    public int? PontaEsquerda => EstaVazio ? null : Pecas[0].ValorA;
    public int? PontaDireita => EstaVazio ? null : Pecas[^1].ValorB;

    // IMPLEMENTADO PELO ALUNO - GAP: validação de compatibilidade
    // Verifica se a peça pode ser encaixada no lado escolhido
    public bool PodeColar(Peca peca, LadoTabuleiro lado)
    {
        if (EstaVazio) return true;
        int ponta = lado == LadoTabuleiro.Esquerda ? PontaEsquerda!.Value : PontaDireita!.Value;
        return peca.PossuiValor(ponta);
    }

    // IMPLEMENTADO PELO ALUNO - GAP: posicionamento de peças
    // Cola a peça no tabuleiro, invertendo se necessário
    public void Colar(Peca peca, LadoTabuleiro lado)
    {
        if (!PodeColar(peca, lado))
            throw new JogadaInvalidaException($"Não é possível colar a peça {peca} no lado {lado}.");

        Peca pecaParaColar = peca;
        if (!EstaVazio)
        {
            int ponta = lado == LadoTabuleiro.Esquerda ? PontaEsquerda!.Value : PontaDireita!.Value;
            // Inverte a peça se o valor compatível estiver em ValorB
            if (peca.ValorB == ponta && peca.ValorA != ponta)
                pecaParaColar = peca.Inverter();
        }

        if (lado == LadoTabuleiro.Esquerda)
            Pecas.Insert(0, pecaParaColar);
        else
            Pecas.Add(pecaParaColar);
    }

    public int SomarPontasExternas() =>
        EstaVazio ? 0 : PontaEsquerda!.Value + PontaDireita!.Value;

    // IMPLEMENTADO PELO ALUNO - GAP: verificação de travamento
    // Retorna true se nenhum jogador tem peça compatível
    public bool EstaTravado(IEnumerable<MaoJogador> maosJogadores)
    {
        if (EstaVazio) return false;
        foreach (var mao in maosJogadores)
            foreach (var peca in mao.ObterPecas())
                if (PodeColar(peca, LadoTabuleiro.Esquerda) || PodeColar(peca, LadoTabuleiro.Direita))
                    return false;
        return true;
    }

    public void Limpar() => Pecas.Clear();
}