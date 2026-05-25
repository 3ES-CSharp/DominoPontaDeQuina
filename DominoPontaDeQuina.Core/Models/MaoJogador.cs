using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IMaoJogador"/>
public class MaoJogador(Jogador jogador) : IMaoJogador
{
    /// <summary>
    /// Obtem as pecas atualmente armazenadas na mao do jogador.
    /// </summary>
    public readonly List<Peca> _pecas = [];

    /// <inheritdoc />
    public Jogador Jogador { get; } = jogador ?? throw new ArgumentNullException(nameof(jogador));

    /// <inheritdoc />
    public void AdicionarPeca(Peca peca) => _pecas.Add(peca);

    /// <inheritdoc />
    public int SomarPecasNaMao() => _pecas.Sum(peca => peca.SomaValores);

    /// <inheritdoc />
    public bool PossuiSena() => _pecas.Any(peca => peca.EhSena);

    /// <inheritdoc />
    public bool EstaSemPecas() => _pecas.Count == 0;

    /// <inheritdoc />
    public Jogada GetJogada(Tabuleiro tabuleiro)
    {
        foreach (var peca in _pecas)
        {
            if (tabuleiro.PodeColar(peca, LadoTabuleiro.Direita))
            {
                _pecas.Remove(peca);
                return new Jogada(Jogador, peca, peca.ValorA, LadoTabuleiro.Direita);
            }
        }

        foreach (var peca in _pecas)
        {
            if (tabuleiro.PodeColar(peca, LadoTabuleiro.Esquerda))
            {
                _pecas.Remove(peca);
                return new Jogada(Jogador, peca, peca.ValorA, LadoTabuleiro.Esquerda);
            }
        }

        return new Jogada(Jogador);
    }

    /// <inheritdoc />
    public void DefazerJogada(Jogada jogada)
    {
        if (jogada.EhPassarVez()) return;

        if(jogada.Peca != null) _pecas.Add(jogada.Peca.Value);
    }
}