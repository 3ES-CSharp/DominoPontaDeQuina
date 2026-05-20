using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Services;
using DominoPontaDeQuina.Core.Validators;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IMaoJogador"/>
public class MaoJogador(Jogador jogador) : IMaoJogador
{
    /// <summary>
    /// Armazena internamente as pecas atualmente na mao do jogador.
    /// A colecao e somente para uso interno; o acesso publico ocorre por <see cref="Pecas"/>.
    /// </summary>
    readonly List<Peca> _pecas = [];

    /// <summary>
    /// Obtem uma visao somente leitura das pecas na mao do jogador.
    /// Util para consultas em validadores e servicos sem expor a colecao mutavel.
    /// </summary>
    public IReadOnlyCollection<Peca> Pecas => _pecas.AsReadOnly();

    /// <inheritdoc />
    public Jogador Jogador { get; } = jogador ?? throw new ArgumentNullException(nameof(jogador));

    /// <inheritdoc />
    public void AdicionarPeca(Peca peca) => _pecas.Add(peca);

    /// <summary>
    /// Remove uma peca especifica da mao do jogador.
    /// Operacao usada principalmente apos a peca ser colada no tabuleiro durante uma jogada.
    /// </summary>
    /// <param name="peca">A peca a ser removida da mao.</param>
    public void RemoverPeca(Peca peca) => _pecas.Remove(peca);

    /// <inheritdoc />
    public int SomarPecasNaMao() => _pecas.Sum(peca => peca.SomaValores);

    /// <inheritdoc />
    public bool PossuiSena() => _pecas.Any(peca => peca.EhSena);

    /// <inheritdoc />
    public bool EstaSemPecas() => _pecas.Count == 0;

    /// <summary>
    /// Determina a jogada que sera realizada a partir do estado atual do tabuleiro.
    /// A regra de selecao da peca e a manipulacao da mao sao delegadas ao <see cref="JogadaService"/>,
    /// mantendo esta classe focada em representar a mao do jogador.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual da rodada.</param>
    /// <returns>A jogada escolhida ou uma jogada de passagem de vez quando nenhuma peca e compativel.</returns>
    public Jogada GetJogada(Tabuleiro tabuleiro) =>
        JogadaService.DecidirJogada(this, tabuleiro);

    /// <summary>
    /// Desfaz os efeitos de uma jogada sobre a mao, devolvendo a peca utilizada quando aplicavel.
    /// A decisao e tomada com base no <see cref="MaoJogadorValidator"/> para concentrar a regra de negocio.
    /// </summary>
    /// <param name="jogada">A jogada que deve ser desfeita.</param>
    public void DefazerJogada(Jogada jogada)
    {
        ArgumentNullException.ThrowIfNull(jogada);

        if (MaoJogadorValidator.DeveDevolverPeca(jogada))
        {
            AdicionarPeca(jogada.Peca!.Value);
        }
    }
}
