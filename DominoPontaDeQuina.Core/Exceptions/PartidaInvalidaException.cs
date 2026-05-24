namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Lançada quando uma operação não é permitida no estado atual da partida ou rodada.
/// </summary>
/// <param name="message">Mensagem descritiva sobre a operação inválida.</param>
public class PartidaInvalidaException(string message) : DominoDomainException(message)
{
}
