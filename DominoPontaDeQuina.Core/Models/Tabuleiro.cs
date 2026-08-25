using DominoPontaDeQuina.Core.Enums;

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
    /// A regra esperada e validar se a peca possui valor compativel com a ponta externa do lado escolhido.
    /// </summary>
    /// <param name="peca">A peca a ser verificada.</param>
    /// <param name="lado">O lado do tabuleiro.</param>
    /// <returns><see langword="true"/> quando a peca puder ser colada; caso contrario, <see langword="false"/>.</returns>
    public bool PodeColar(Peca peca, LadoTabuleiro lado)
    {
        // Tabuleiro vazio: qualquer peca pode ser colada em qualquer lado
        if (EstaVazio)
            return true;

        // Verifica se a peca possui o valor compativel com a ponta do lado escolhido
        return lado == LadoTabuleiro.Esquerda
            ? peca.PossuiValor(PontaEsquerda!.Value)
            : peca.PossuiValor(PontaDireita!.Value);
    }

    /// <summary>
    /// Cola uma peca no lado informado do tabuleiro.
    /// A regra esperada e posicionar a peca no lado correto, invertendo seus valores quando necessario.
    /// </summary>
    /// <param name="peca">A peca a ser colada.</param>
    /// <param name="lado">O lado do tabuleiro.</param>
    public void Colar(Peca peca, LadoTabuleiro lado)
    {
        if (EstaVazio)
        {
            // Primeira peca: insere diretamente
            Pecas.Add(peca);
            return;
        }

        if (lado == LadoTabuleiro.Esquerda)
        {
            // O ValorB da peca deve encaixar com a ponta esquerda do tabuleiro
            // para que o ValorA da peca fique exposto como nova ponta esquerda
            if (peca.ValorB != PontaEsquerda!.Value)
                peca = peca.Inverter();
            Pecas.Insert(0, peca);
        }
        else // LadoTabuleiro.Direita
        {
            // O ValorA da peca deve encaixar com a ponta direita do tabuleiro
            // para que o ValorB da peca fique exposto como nova ponta direita
            if (peca.ValorA != PontaDireita!.Value)
                peca = peca.Inverter();
            Pecas.Add(peca);
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
    /// Determina se o tabuleiro esta travado.
    /// O travamento e esperado quando nenhuma mao de jogador possuir peca compativel com as pontas externas atuais.
    /// </summary>
    /// <param name="maosJogadores">As maos dos jogadores da rodada.</param>
    /// <returns><see langword="true"/> quando o tabuleiro estiver travado; caso contrario, <see langword="false"/>.</returns>
    public bool EstaTravado(IEnumerable<MaoJogador> maosJogadores)
    {
        // Tabuleiro vazio nunca esta travado
        if (EstaVazio)
            return false;

        // Travado quando nenhuma mao consegue colar em nenhum dos dois lados
        return maosJogadores.All(mao =>
            !mao.Pecas.Any(peca => PodeColar(peca, LadoTabuleiro.Esquerda)) &&
            !mao.Pecas.Any(peca => PodeColar(peca, LadoTabuleiro.Direita)));
    }

    /// <summary>
    /// Limpa o tabuleiro para preparar uma nova rodada.
    /// </summary>
    public void Limpar() =>
        Pecas.Clear();
}