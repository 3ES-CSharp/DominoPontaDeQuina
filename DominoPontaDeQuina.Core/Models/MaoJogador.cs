using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Services;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IMaoJogador"/>
public class MaoJogador(Jogador jogador) : IMaoJogador
{
    /// <summary>
    /// Obtem as pecas atualmente armazenadas na mao do jogador.
    /// </summary>
    List<Peca> _pecas = [];

    /// <summary>
    /// Obtem uma visao somente leitura das pecas na mao do jogador.
    /// </summary>
    public IReadOnlyCollection<Peca> Pecas => _pecas.AsReadOnly();

    /// <inheritdoc />
    public Jogador Jogador { get; } = jogador ?? throw new ArgumentNullException(nameof(jogador));

    /// <inheritdoc />
    public void AdicionarPeca(Peca peca) => _pecas.Add(peca);

    /// <summary>
    /// Adiciona uma peça internamente, sem expor a regra fora do core.
    /// </summary>
    internal void AdicionarPecaInterno(Peca peca) => _pecas.Add(peca);

    /// <summary>
    /// Remove uma peça internamente da mão.
    /// </summary>
    internal void RemoverPeca(Peca peca) => _pecas.Remove(peca);

    /// <inheritdoc />
    public int SomarPecasNaMao() => _pecas.Sum(peca => peca.SomaValores);

    /// <inheritdoc />
    public bool PossuiSena() => _pecas.Any(peca => peca.EhSena);

    /// <inheritdoc />
    public bool EstaSemPecas() => _pecas.Count == 0;

    /// <inheritdoc />
    public Jogada GetJogada(Tabuleiro tabuleiro) => MaoJogadorService.GetJogada(this, tabuleiro);

    /// <inheritdoc />
    public void DefazerJogada(Jogada jogada) => MaoJogadorService.DefazerJogada(this, jogada);
}