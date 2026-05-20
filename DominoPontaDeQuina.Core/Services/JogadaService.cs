using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Models;
using DominoPontaDeQuina.Core.Validators;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Servico responsavel por construir e aplicar jogadas a partir do estado atual do tabuleiro.
/// Concentra a logica de selecao de pecas e a regra de orientacao dos valores ao colar pecas.
/// </summary>
public static class JogadaService
{
    /// <summary>
    /// Tenta encontrar uma jogada compativel para a mao do jogador a partir do estado atual do tabuleiro.
    /// A primeira peca compativel encontrada (verificando primeiro o lado direito e depois o esquerdo)
    /// e removida da mao e usada para compor a jogada.
    /// </summary>
    /// <param name="mao">A mao do jogador que deve decidir a jogada.</param>
    /// <param name="tabuleiro">O tabuleiro atual da rodada.</param>
    /// <returns>A jogada escolhida ou uma jogada de passagem de vez quando nenhuma peca e compativel.</returns>
    public static Jogada DecidirJogada(MaoJogador mao, Tabuleiro tabuleiro)
    {
        ArgumentNullException.ThrowIfNull(mao);
        ArgumentNullException.ThrowIfNull(tabuleiro);

        if (mao.EstaSemPecas())
        {
            return CriarJogadaPassarVez(mao.Jogador);
        }

        foreach (var peca in mao.Pecas)
        {
            if (TabuleiroValidator.PodeColar(tabuleiro, peca, LadoTabuleiro.Direita))
            {
                int valorColado = ObterValorColado(peca, tabuleiro.PontaDireita, ladoEsquerdo: false);
                mao.RemoverPeca(peca);
                return new Jogada(mao.Jogador, peca, valorColado, LadoTabuleiro.Direita);
            }

            if (TabuleiroValidator.PodeColar(tabuleiro, peca, LadoTabuleiro.Esquerda))
            {
                int valorColado = ObterValorColado(peca, tabuleiro.PontaEsquerda, ladoEsquerdo: true);
                mao.RemoverPeca(peca);
                return new Jogada(mao.Jogador, peca, valorColado, LadoTabuleiro.Esquerda);
            }
        }

        return CriarJogadaPassarVez(mao.Jogador);
    }

    /// <summary>
    /// Cola uma peca em um dos lados do tabuleiro, invertendo seus valores quando o encaixe exigir.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro alvo da operacao.</param>
    /// <param name="peca">A peca a ser colada.</param>
    /// <param name="lado">O lado escolhido para o encaixe.</param>
    /// <exception cref="JogadaInvalidaException">Lancada quando a peca nao e compativel com o lado escolhido.</exception>
    public static void Colar(Tabuleiro tabuleiro, Peca peca, LadoTabuleiro lado)
    {
        ArgumentNullException.ThrowIfNull(tabuleiro);

        if (!TabuleiroValidator.PodeColar(tabuleiro, peca, lado))
        {
            throw new JogadaInvalidaException(peca, lado);
        }

        if (tabuleiro.EstaVazio)
        {
            tabuleiro.Pecas.Add(peca);
            return;
        }

        if (lado == LadoTabuleiro.Esquerda)
        {
            if (peca.ValorA == tabuleiro.PontaEsquerda!.Value)
            {
                peca = peca.Inverter();
            }
            tabuleiro.Pecas.Insert(0, peca);
        }
        else
        {
            if (peca.ValorB == tabuleiro.PontaDireita!.Value)
            {
                peca = peca.Inverter();
            }
            tabuleiro.Pecas.Insert(tabuleiro.Pecas.Count, peca);
        }
    }

    /// <summary>
    /// Determina qual dos valores da peca sera o valor de encaixe com a ponta do tabuleiro.
    /// </summary>
    /// <param name="peca">A peca avaliada.</param>
    /// <param name="ponta">O valor exposto da ponta no lado escolhido.</param>
    /// <param name="ladoEsquerdo">Indica se a ponta avaliada e a esquerda.</param>
    /// <returns>O valor da peca que sera colado contra a ponta informada.</returns>
    private static int ObterValorColado(Peca peca, int? ponta, bool ladoEsquerdo)
    {
        // Mantem o comportamento original: quando o valor "A" da peca casa com a ponta, retorna esse valor;
        // caso contrario, retorna o valor "B".
        return ponta == peca.ValorA ? peca.ValorA : peca.ValorB;
    }

    /// <summary>
    /// Cria uma jogada que apenas representa a passagem de vez do jogador.
    /// </summary>
    /// <param name="jogador">O jogador que esta passando a vez.</param>
    /// <returns>Uma jogada sem peca e sem lado, indicando passagem.</returns>
    private static Jogada CriarJogadaPassarVez(Jogador jogador) =>
        new(jogador, null, null, null);
}
