using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IMaoJogador"/>
public class MaoJogador(Jogador jogador) : IMaoJogador
{
    /// <summary>
    /// Obtem as pecas atualmente armazenadas na mao do jogador.
    /// </summary>
    List<Peca> _pecas = [];

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
        if (tabuleiro.EstaVazio)
        {
            var peca = _pecas[0];
            _pecas.RemoveAt(0);
            return new Jogada(Jogador, peca, null, LadoTabuleiro.Direita);
        }

        var indiceEsquerda = _pecas.FindIndex(p => tabuleiro.PodeColar(p, LadoTabuleiro.Esquerda));
        if (indiceEsquerda >= 0)
        {
            var peca = _pecas[indiceEsquerda];
            _pecas.RemoveAt(indiceEsquerda);
            return new Jogada(Jogador, peca, null, LadoTabuleiro.Esquerda);
        }

        var indiceDireita = _pecas.FindIndex(p => tabuleiro.PodeColar(p, LadoTabuleiro.Direita));
        if (indiceDireita >= 0)
        {
            var peca = _pecas[indiceDireita];
            _pecas.RemoveAt(indiceDireita);
            return new Jogada(Jogador, peca, null, LadoTabuleiro.Direita);
        }

        return new Jogada(Jogador);
    }

    /// <inheritdoc />
    public void DefazerJogada(Jogada jogada)
    {
        if (!jogada.EhPassarVez() && jogada.Peca.HasValue)
            _pecas.Add(jogada.Peca.Value);
    }

    /// <summary>
    /// Verifica se a mão possui alguma peça compatível com as pontas atuais do tabuleiro.
    /// Usado para determinar se o jogador pode realizar uma jogada ou se o tabuleiro está travado.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual da rodada.</param>
    /// <returns><see langword="true"/> quando houver peça compatível; caso contrário, <see langword="false"/>.</returns>
    internal bool PossuiPecaCompativel(Tabuleiro tabuleiro) =>
        _pecas.Any(p =>
            tabuleiro.PodeColar(p, LadoTabuleiro.Esquerda) ||
            tabuleiro.PodeColar(p, LadoTabuleiro.Direita));
}
