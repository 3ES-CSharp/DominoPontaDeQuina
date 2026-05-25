// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583) - Grupo 3ESPF/05
using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Enums;

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

    /// <summary>
    /// Obtem as pecas atualmente armazenadas na mao do jogador de forma segura (somente leitura).
    /// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583)
    /// </summary>
    public IReadOnlyList<Peca> Pecas => _pecas.AsReadOnly();

    /// <inheritdoc />
    /// <summary>
    /// Obtem a jogada escolhida a partir do estado atual do tabuleiro.
    /// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583)
    /// </summary>
    public Jogada GetJogada(Tabuleiro tabuleiro)
    {
        // Tenta encontrar uma peça compatível para colar no lado Direito
        for (int i = 0; i < _pecas.Count; i++)
        {
            var peca = _pecas[i];
            if (tabuleiro.PodeColar(peca, LadoTabuleiro.Direita))
            {
                _pecas.RemoveAt(i);
                return new Jogada(Jogador, peca, valorColado: tabuleiro.PontaDireita, LadoTabuleiro.Direita);
            }
        }

        // Tenta encontrar uma peça compatível para colar no lado Esquerdo
        for (int i = 0; i < _pecas.Count; i++)
        {
            var peca = _pecas[i];
            if (tabuleiro.PodeColar(peca, LadoTabuleiro.Esquerda))
            {
                _pecas.RemoveAt(i);
                return new Jogada(Jogador, peca, valorColado: tabuleiro.PontaEsquerda, LadoTabuleiro.Esquerda);
            }
        }

        // Caso nenhuma peça seja compatível, o jogador passa a vez
        return new Jogada(Jogador);
    }

    /// <inheritdoc />
    /// <summary>
    /// Desfaz os efeitos de uma jogada sobre a mao do jogador, devolvendo a peca caso tenha sido usada.
    /// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583)
    /// </summary>
    public void DefazerJogada(Jogada jogada)
    {
        if (jogada == null)
            return;

        if (jogada.EhPassarVez() || jogada.Peca is null)
            return;

        _pecas.Add(jogada.Peca.Value);
    }
}