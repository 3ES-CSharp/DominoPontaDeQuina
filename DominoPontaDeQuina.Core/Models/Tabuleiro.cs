using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Exceptions;
using DominoPontaDeQuina.Core.Validators;

namespace DominoPontaDeQuina.Core.Models;

/// <summary>
/// Representa o tabuleiro no nivel da rodada dentro da hierarquia Partida -> Rodadas -> Jogadas.
/// Neste nivel ficam as pecas ja coladas e as informacoes necessarias para validar jogadas,
/// calcular pontuacao pelas pontas externas e verificar situacoes de travamento.
/// </summary>
public class Tabuleiro
{
    /// <summary>
    /// Obtem as pecas posicionadas no tabuleiro na ordem em que foram coladas.
    /// </summary>
    public List<Peca> Pecas { get; } = [];

    /// <summary>
    /// Indica se o tabuleiro ainda nao possui pecas coladas.
    /// </summary>
    public bool EstaVazio => Pecas.Count == 0;

    /// <summary>
    /// Obtem a ponta esquerda atualmente exposta no tabuleiro.
    /// Quando o tabuleiro estiver vazio, nao existe ponta externa disponivel.
    /// </summary>
    public int? PontaEsquerda => EstaVazio ? null : Pecas[0].ValorA;

    /// <summary>
    /// Obtem a ponta direita atualmente exposta no tabuleiro.
    /// Quando o tabuleiro estiver vazio, nao existe ponta externa disponivel.
    /// </summary>
    public int? PontaDireita => EstaVazio ? null : Pecas[^1].ValorB;

    /// <summary>
    /// Determina se uma peca pode ser colada no lado informado.
    /// A regra de compatibilidade e delegada ao <see cref="ColagemValidator"/>.
    /// </summary>
    /// <param name="peca">A peca a ser verificada.</param>
    /// <param name="lado">O lado do tabuleiro.</param>
    /// <returns><see langword="true"/> quando a peca puder ser colada; caso contrario, <see langword="false"/>.</returns>
    public bool PodeColar(Peca peca, LadoTabuleiro lado) =>
        ColagemValidator.PodeColar(this, peca, lado);

    /// <summary>
    /// Cola uma peca no lado informado do tabuleiro, invertendo seus valores quando necessario para encaixar na ponta.
    /// </summary>
    /// <param name="peca">A peca a ser colada.</param>
    /// <param name="lado">O lado do tabuleiro.</param>
    /// <exception cref="JogadaInvalidaException">Lancada quando a peca nao e compativel com o lado escolhido.</exception>
    public void Colar(Peca peca, LadoTabuleiro lado)
    {
        if (!PodeColar(peca, lado))
            throw new JogadaInvalidaException(
                $"A peca {peca} nao pode ser colada no lado {lado} do tabuleiro.");

        if (EstaVazio)
        {
            Pecas.Add(peca);
            return;
        }

        if (lado == LadoTabuleiro.Esquerda)
        {
            var pecaParaInserir = peca.ValorB == PontaEsquerda!.Value ? peca : peca.Inverter();
            Pecas.Insert(0, pecaParaInserir);
        }
        else
        {
            var pecaParaInserir = peca.ValorA == PontaDireita!.Value ? peca : peca.Inverter();
            Pecas.Add(pecaParaInserir);
        }
    }

    /// <summary>
    /// Soma os valores das pontas externas atualmente expostas.
    /// Essa soma e a base para regras de pontuacao em que a rodada concede pontos quando o resultado for multiplo de 5.
    /// </summary>
    /// <returns>A soma das pontas externas, ou 0 quando o tabuleiro estiver vazio.</returns>
    public int SomarPontasExternas() =>
        EstaVazio ? 0 : PontaEsquerda!.Value + PontaDireita!.Value;

    /// <summary>
    /// Determina se o tabuleiro esta travado, ou seja, se nenhuma mao informada possui peca compativel
    /// com qualquer uma das pontas externas atuais.
    /// </summary>
    /// <param name="maosJogadores">As maos dos jogadores da rodada.</param>
    /// <returns><see langword="true"/> quando o tabuleiro estiver travado; caso contrario, <see langword="false"/>.</returns>
    public bool EstaTravado(IEnumerable<MaoJogador> maosJogadores)
    {
        ArgumentNullException.ThrowIfNull(maosJogadores);

        if (EstaVazio)
            return false;

        foreach (var mao in maosJogadores)
        {
            if (PossuiPecaCompativel(mao))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Limpa o tabuleiro para preparar uma nova rodada.
    /// </summary>
    public void Limpar() =>
        Pecas.Clear();

    /// <summary>
    /// Indica se a mao informada possui ao menos uma peca compativel com alguma das pontas do tabuleiro.
    /// </summary>
    /// <param name="mao">A mao avaliada.</param>
    /// <returns><see langword="true"/> quando existir peca compativel; caso contrario, <see langword="false"/>.</returns>
    private bool PossuiPecaCompativel(MaoJogador mao)
    {
        foreach (var peca in mao.Pecas)
        {
            if (ColagemValidator.PodeColarEmAlgumLado(this, peca))
                return true;
        }

        return false;
    }
}
