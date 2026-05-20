using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Validators;

/// <summary>
/// Concentra as validacoes relacionadas ao estado do tabuleiro e a possibilidade de encaixe de pecas.
/// A separacao em um validator dedicado mantem a classe <see cref="Tabuleiro"/> focada em representar o estado,
/// enquanto as regras de validacao podem ser reutilizadas e testadas independentemente.
/// </summary>
public static class TabuleiroValidator
{
    /// <summary>
    /// Determina se uma peca pode ser colada no lado informado do tabuleiro.
    /// Quando o tabuleiro estiver vazio, qualquer peca e considerada compativel.
    /// Caso contrario, a peca precisa possuir um valor igual ao da ponta externa do lado escolhido.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro consultado.</param>
    /// <param name="peca">A peca a ser verificada.</param>
    /// <param name="lado">O lado do tabuleiro alvo do encaixe.</param>
    /// <returns><see langword="true"/> quando o encaixe e valido; caso contrario, <see langword="false"/>.</returns>
    public static bool PodeColar(Tabuleiro tabuleiro, Peca peca, LadoTabuleiro lado)
    {
        ArgumentNullException.ThrowIfNull(tabuleiro);

        if (tabuleiro.EstaVazio)
        {
            return true;
        }

        return lado == LadoTabuleiro.Esquerda
            ? peca.PossuiValor(tabuleiro.PontaEsquerda!.Value)
            : peca.PossuiValor(tabuleiro.PontaDireita!.Value);
    }

    /// <summary>
    /// Determina se o tabuleiro esta travado considerando todas as maos dos jogadores ativos.
    /// O tabuleiro e considerado travado quando ja existem pecas coladas e nenhum jogador
    /// possui peca compativel com qualquer uma das pontas externas.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro consultado.</param>
    /// <param name="maosJogadores">As maos dos jogadores ainda ativos na rodada.</param>
    /// <returns><see langword="true"/> quando o tabuleiro estiver travado; caso contrario, <see langword="false"/>.</returns>
    public static bool EstaTravado(Tabuleiro tabuleiro, IEnumerable<MaoJogador> maosJogadores)
    {
        ArgumentNullException.ThrowIfNull(tabuleiro);
        ArgumentNullException.ThrowIfNull(maosJogadores);

        if (tabuleiro.EstaVazio)
            return false;

        foreach (var mao in maosJogadores)
        {
            foreach (var peca in mao.Pecas)
            {
                if (PodeColar(tabuleiro, peca, LadoTabuleiro.Esquerda)
                    || PodeColar(tabuleiro, peca, LadoTabuleiro.Direita))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
