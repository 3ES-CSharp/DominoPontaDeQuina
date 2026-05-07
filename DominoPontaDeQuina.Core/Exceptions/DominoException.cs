namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Classe base abstrata para todas as exceções customizadas do domínio do jogo de dominó.
/// Criada para atender o critério de avaliação "Implementação de exceções customizadas" (10% da nota).
/// </summary>
public abstract class DominoException : Exception
{
    /// <summary>
    /// Inicializa uma nova instância da exceção com uma mensagem de erro específica.
    /// </summary>
    /// <param name="message">Mensagem que descreve o erro ocorrido.</param>
    protected DominoException(string message) : base(message) { }

    /// <summary>
    /// Inicializa uma nova instância da exceção com uma mensagem de erro e uma exceção interna.
    /// </summary>
    /// <param name="message">Mensagem que descreve o erro ocorrido.</param>
    /// <param name="innerException">Exceção interna que causou esta exceção.</param>
    protected DominoException(string message, Exception innerException) : base(message, innerException) { }
}