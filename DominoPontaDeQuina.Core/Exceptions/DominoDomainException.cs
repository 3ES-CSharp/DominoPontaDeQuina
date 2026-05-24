namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Classe base para exceções do domínio do jogo de dominó.
/// Todas as exceções específicas do domínio devem herdar desta classe.
/// </summary>
/// <param name="message">Mensagem descritiva do erro de domínio.</param>
public class DominoDomainException(string message) : InvalidOperationException(message)
{
}
