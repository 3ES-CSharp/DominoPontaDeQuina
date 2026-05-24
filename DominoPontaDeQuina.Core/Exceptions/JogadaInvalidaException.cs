namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Lançada quando uma jogada não é válida no contexto atual do tabuleiro ou da rodada.
/// </summary>
/// <param name="message">Mensagem descritiva sobre a jogada inválida.</param>
public class JogadaInvalidaException(string message) : DominoDomainException(message)
{
}
