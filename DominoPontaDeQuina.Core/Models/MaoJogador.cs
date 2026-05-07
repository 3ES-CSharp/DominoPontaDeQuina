using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Services;

namespace DominoPontaDeQuina.Core.Models;

// CLASSE MODIFICADA PELO ALUNO - GRUPO 01
// Gaps implementados: GetJogada() e DefazerJogada()
public class MaoJogador(Jogador jogador) : IMaoJogador
{
    private List<Peca> _pecas = [];
    private readonly IJogadaValidator _jogadaValidator = new JogadaValidator();

    // Propriedade adicionada pelo aluno - retorna cópia das peças
    public IReadOnlyList<Peca> Pecas => _pecas.AsReadOnly();

    // Propriedade adicionada pelo aluno - quantidade de peças na mão
    public int QuantidadePecas => _pecas.Count;

    public Jogador Jogador { get; } = jogador ?? throw new ArgumentNullException(nameof(jogador));

    public void AdicionarPeca(Peca peca) => _pecas.Add(peca);
    public int SomarPecasNaMao() => _pecas.Sum(p => p.SomaValores);
    public bool PossuiSena() => _pecas.Any(p => p.EhSena);
    public bool EstaSemPecas() => _pecas.Count == 0;

    // Método adicionado pelo aluno - verifica se possui uma peça específica
    public bool PossuiPeca(Peca peca) => _pecas.Contains(peca);

    // Método adicionado pelo aluno - retorna cópia das peças
    public IEnumerable<Peca> ObterPecas() => _pecas.ToList();

    // Método adicionado pelo aluno - remove peça após jogar
    public bool RemoverPeca(Peca peca) => _pecas.Remove(peca);

    // IMPLEMENTADO PELO ALUNO - GAP: decisão da jogada
    // Escolhe a PRIMEIRA peça compatível encontrada (estratégia simplificada)
    public Jogada GetJogada(Tabuleiro tabuleiro)
    {
        if (_jogadaValidator.PossuiPecaCompativel(this, tabuleiro))
        {
            foreach (var peca in _pecas)
            {
                if (tabuleiro.PodeColar(peca, LadoTabuleiro.Esquerda))
                {
                    int valorColado = ObterValorColado(peca, tabuleiro, LadoTabuleiro.Esquerda);
                    return new Jogada(Jogador, peca, valorColado, LadoTabuleiro.Esquerda);
                }
                if (tabuleiro.PodeColar(peca, LadoTabuleiro.Direita))
                {
                    int valorColado = ObterValorColado(peca, tabuleiro, LadoTabuleiro.Direita);
                    return new Jogada(Jogador, peca, valorColado, LadoTabuleiro.Direita);
                }
            }
        }
        return new Jogada(Jogador); // Passa a vez
    }

    // Método auxiliar adicionado pelo aluno
    private int ObterValorColado(Peca peca, Tabuleiro tabuleiro, LadoTabuleiro lado)
    {
        int ponta = lado == LadoTabuleiro.Esquerda ? tabuleiro.PontaEsquerda!.Value : tabuleiro.PontaDireita!.Value;
        return peca.ValorA == ponta ? peca.ValorB : peca.ValorA;
    }

    // IMPLEMENTADO PELO ALUNO - GAP: desfazer jogada (rollback)
    public void DefazerJogada(Jogada jogada)
    {
        if (jogada.Peca.HasValue && !_pecas.Contains(jogada.Peca.Value))
            _pecas.Add(jogada.Peca.Value);
    }
}