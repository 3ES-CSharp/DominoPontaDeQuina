using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Services;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IMaoJogador"/>
public class MaoJogador(Jogador jogador) : IMaoJogador
{
    private List<Peca> _pecas = [];
    private readonly IJogadaValidator _jogadaValidator = new JogadaValidator();

    /// <summary>
    /// Obtém uma cópia somente leitura das peças na mão do jogador.
    /// </summary>
    public IReadOnlyList<Peca> Pecas => _pecas.AsReadOnly();

    /// <summary>
    /// Obtém a quantidade de peças atualmente na mão.
    /// </summary>
    public int QuantidadePecas => _pecas.Count;

    /// <inheritdoc />
    public Jogador Jogador { get; } = jogador ?? throw new ArgumentNullException(nameof(jogador));

    /// <inheritdoc />
    public void AdicionarPeca(Peca peca) => _pecas.Add(peca);

    /// <inheritdoc />
    public int SomarPecasNaMao() => _pecas.Sum(p => p.SomaValores);

    /// <inheritdoc />
    public bool PossuiSena() => _pecas.Any(p => p.EhSena);

    /// <inheritdoc />
    public bool EstaSemPecas() => _pecas.Count == 0;

    /// <summary>
    /// Verifica se o jogador possui uma peça específica na mão.
    /// </summary>
    public bool PossuiPeca(Peca peca) => _pecas.Contains(peca);

    /// <summary>
    /// Retorna uma cópia de todas as peças da mão.
    /// </summary>
    public IEnumerable<Peca> ObterPecas() => _pecas.ToList();

    /// <summary>
    /// Remove uma peça da mão do jogador (usado quando a peça é jogada).
    /// </summary>
    public bool RemoverPeca(Peca peca) => _pecas.Remove(peca);

    /// <inheritdoc />
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
        return new Jogada(Jogador);
    }

    private int ObterValorColado(Peca peca, Tabuleiro tabuleiro, LadoTabuleiro lado)
    {
        int ponta = lado == LadoTabuleiro.Esquerda ? tabuleiro.PontaEsquerda!.Value : tabuleiro.PontaDireita!.Value;
        return peca.ValorA == ponta ? peca.ValorB : peca.ValorA;
    }

    /// <inheritdoc />
    public void DefazerJogada(Jogada jogada)
    {
        if (jogada.Peca.HasValue && !_pecas.Contains(jogada.Peca.Value))
            _pecas.Add(jogada.Peca.Value);
    }
}