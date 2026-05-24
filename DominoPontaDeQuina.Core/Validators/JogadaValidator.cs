using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Validators;

/// <summary>
/// Valida jogadas no contexto atual do tabuleiro, garantindo que apenas jogadas
/// compatíveis com as pontas expostas sejam aceitas.
/// </summary>
public static class JogadaValidator
{
    /// <summary>
    /// Verifica se uma jogada é válida para o estado atual do tabuleiro.
    /// Uma jogada de passar vez é sempre válida. Uma jogada real requer que a peça
    /// seja compatível com a ponta do lado escolhido.
    /// </summary>
    /// <param name="jogada">A jogada a ser validada.</param>
    /// <param name="tabuleiro">O tabuleiro atual da rodada.</param>
    /// <returns><see langword="true"/> quando a jogada for válida; caso contrário, <see langword="false"/>.</returns>
    public static bool EhValida(Jogada jogada, Tabuleiro tabuleiro)
    {
        if (jogada.EhPassarVez())
            return true;

        if (jogada.Peca is null || jogada.Lado is null)
            return false;

        return tabuleiro.PodeColar(jogada.Peca.Value, jogada.Lado.Value);
    }
}
