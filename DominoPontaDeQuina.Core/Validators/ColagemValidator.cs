using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Validators;

/// <summary>
/// Validador responsavel por concentrar a regra de compatibilidade entre uma peca e a ponta do tabuleiro
/// no momento de colar uma peca em um determinado lado.
/// </summary>
/// <remarks>
/// Esta regra e referenciada tanto por <see cref="Tabuleiro"/> (para validar colagens) quanto por
/// <see cref="MaoJogador"/> (para decidir uma jogada compativel). Centraliza-la aqui evita duplicacao
/// e mantem o ponto unico de evolucao caso a regra de compatibilidade mude.
/// </remarks>
public static class ColagemValidator
{
    /// <summary>
    /// Determina se a <paramref name="peca"/> e compativel com o <paramref name="lado"/> escolhido do <paramref name="tabuleiro"/>.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro avaliado.</param>
    /// <param name="peca">A peca candidata a colagem.</param>
    /// <param name="lado">O lado em que se pretende colar a peca.</param>
    /// <returns>
    /// <see langword="true"/> quando o tabuleiro estiver vazio ou quando a peca possuir um valor igual a ponta
    /// externa do lado informado; <see langword="false"/> caso contrario.
    /// </returns>
    public static bool PodeColar(Tabuleiro tabuleiro, Peca peca, LadoTabuleiro lado)
    {
        ArgumentNullException.ThrowIfNull(tabuleiro);

        if (tabuleiro.EstaVazio)
            return true;

        var ponta = lado switch
        {
            LadoTabuleiro.Esquerda => tabuleiro.PontaEsquerda,
            LadoTabuleiro.Direita => tabuleiro.PontaDireita,
            _ => null
        };

        return ponta.HasValue && peca.PossuiValor(ponta.Value);
    }

    /// <summary>
    /// Avalia se a <paramref name="peca"/> e compativel com algum dos dois lados do <paramref name="tabuleiro"/>.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro avaliado.</param>
    /// <param name="peca">A peca candidata.</param>
    /// <returns><see langword="true"/> quando ha compatibilidade em algum dos lados; caso contrario, <see langword="false"/>.</returns>
    public static bool PodeColarEmAlgumLado(Tabuleiro tabuleiro, Peca peca) =>
        PodeColar(tabuleiro, peca, LadoTabuleiro.Esquerda) ||
        PodeColar(tabuleiro, peca, LadoTabuleiro.Direita);
}
