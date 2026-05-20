using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IMaoJogador"/>
public class MaoJogador(Jogador jogador) : IMaoJogador
{
    /// <summary>
    /// Obtem as pecas atualmente armazenadas na mao do jogador.
    /// </summary>
    readonly List<Peca> _pecas = [];

    /// <summary>
    /// Obtem uma visao somente leitura das pecas na mao do jogador.
    /// </summary>
    public IReadOnlyCollection<Peca> Pecas => _pecas.AsReadOnly();

    /// <inheritdoc />
    public Jogador Jogador { get; } = jogador ?? throw new ArgumentNullException(nameof(jogador));

    /// <inheritdoc />
    public void AdicionarPeca(Peca peca) => _pecas.Add(peca);

    /// <inheritdoc />
    public void RemoverPeca(Peca peca) => _pecas.Remove(peca);

    /// <inheritdoc />
    public int SomarPecasNaMao() => _pecas.Sum(peca => peca.SomaValores);

    /// <inheritdoc />
    public bool PossuiSena() => _pecas.Any(peca => peca.EhSena);

    /// <inheritdoc />
    public bool EstaSemPecas() => _pecas.Count == 0;

    /// <inheritdoc />
    public Jogada GetJogada(Tabuleiro tabuleiro)
    {
        // 1. Se o jogador não tiver peças, passa a vez imediatamente.
        if (EstaSemPecas())
        {
            return new Jogada(Jogador, null, null, null);
        }

        // 3. Tratamento para quando já existem peças na mesa
        foreach (var peca in Pecas)
        {
            // Tenta encaixar na Direita
            if (tabuleiro.PodeColar(peca, LadoTabuleiro.Direita))
            {
                int valorColado = (tabuleiro.PontaDireita == peca.ValorA) ? peca.ValorA : peca.ValorB;
                RemoverPeca(peca);

                return new Jogada(Jogador, peca, valorColado, LadoTabuleiro.Direita);
            }

            // Tenta encaixar na Esquerda
            if (tabuleiro.PodeColar(peca, LadoTabuleiro.Esquerda))
            {
                int valorColado = (tabuleiro.PontaEsquerda == peca.ValorA) ? peca.ValorA : peca.ValorB;
                RemoverPeca(peca);

                return new Jogada(Jogador, peca, valorColado, LadoTabuleiro.Esquerda);
            }
        }

        // 4. Se passou por todas as peças e nenhuma serviu, passa a vez
        return new Jogada(Jogador, null, null, null);
    }

    /// <inheritdoc />
    public void DefazerJogada(Jogada jogada)
    {
        // TODO ALUNO: restaurar a mao do jogador ao estado anterior a jogada desfeita.
        if (jogada.Peca is not null && !jogada.EhPassarVez())
            AdicionarPeca(jogada.Peca!.Value);
    }
}