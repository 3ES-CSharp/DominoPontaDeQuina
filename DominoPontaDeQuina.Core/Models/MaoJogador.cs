using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Validators;

namespace DominoPontaDeQuina.Core.Models;

/// <inheritdoc cref="IMaoJogador"/>
public class MaoJogador(Jogador jogador) : IMaoJogador
{
    /// <summary>
    /// Armazena as pecas atualmente disponiveis na mao do jogador.
    /// O nome do campo e preservado por ser referenciado via reflection nos testes.
    /// </summary>
    private List<Peca> _pecas = [];

    /// <inheritdoc />
    public Jogador Jogador { get; } = jogador ?? throw new ArgumentNullException(nameof(jogador));

    /// <summary>
    /// Expoe uma visao somente leitura das pecas presentes na mao.
    /// </summary>
    public IReadOnlyList<Peca> Pecas => _pecas;

    /// <inheritdoc />
    public void AdicionarPeca(Peca peca) => _pecas.Add(peca);

    /// <inheritdoc />
    public int SomarPecasNaMao() => _pecas.Sum(peca => peca.SomaValores);

    /// <inheritdoc />
    public bool PossuiSena() => _pecas.Any(peca => peca.EhSena);

    /// <inheritdoc />
    public bool EstaSemPecas() => _pecas.Count == 0;

    /// <inheritdoc />
    /// <remarks>
    /// A estrategia atual procura primeiro uma peca compativel com a ponta direita do tabuleiro
    /// e, caso nao haja, busca uma peca compativel com a ponta esquerda. Em ambos os casos a peca
    /// escolhida e removida da mao para refletir a aplicacao da jogada. Quando nenhuma peca for
    /// compativel, a jogada retornada representa passar a vez e a mao permanece inalterada.
    /// </remarks>
    public Jogada GetJogada(Tabuleiro tabuleiro)
    {
        ArgumentNullException.ThrowIfNull(tabuleiro);

        var jogada = TentarJogarNoLado(tabuleiro, LadoTabuleiro.Direita)
                  ?? TentarJogarNoLado(tabuleiro, LadoTabuleiro.Esquerda);

        return jogada ?? new Jogada(Jogador);
    }

    /// <inheritdoc />
    public void DefazerJogada(Jogada jogada)
    {
        ArgumentNullException.ThrowIfNull(jogada);

        if (jogada.EhPassarVez() || jogada.Peca is null)
            return;

        _pecas.Add(jogada.Peca.Value);
    }

    /// <summary>
    /// Tenta encontrar uma peca compativel com o lado informado e construir a jogada correspondente.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro consultado.</param>
    /// <param name="lado">O lado avaliado.</param>
    /// <returns>A jogada gerada ou <see langword="null"/> quando nao houver peca compativel.</returns>
    private Jogada? TentarJogarNoLado(Tabuleiro tabuleiro, LadoTabuleiro lado)
    {
        for (var i = 0; i < _pecas.Count; i++)
        {
            var peca = _pecas[i];

            if (!ColagemValidator.PodeColar(tabuleiro, peca, lado))
                continue;

            var valorColado = ObterValorColado(tabuleiro, peca, lado);
            _pecas.RemoveAt(i);

            return new Jogada(Jogador, peca, valorColado, lado);
        }

        return null;
    }

    /// <summary>
    /// Determina o valor que sera efetivamente conectado a ponta do tabuleiro.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro consultado.</param>
    /// <param name="peca">A peca a ser colada.</param>
    /// <param name="lado">O lado escolhido.</param>
    /// <returns>O valor a ser conectado, ou <see langword="null"/> quando o tabuleiro estiver vazio.</returns>
    private static int? ObterValorColado(Tabuleiro tabuleiro, Peca peca, LadoTabuleiro lado)
    {
        if (tabuleiro.EstaVazio)
            return null;

        return lado == LadoTabuleiro.Esquerda
            ? tabuleiro.PontaEsquerda
            : tabuleiro.PontaDireita;
    }
}
