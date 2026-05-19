namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Excecao base do dominio do jogo Domino Ponta de Quina.
/// Todas as excecoes especificas do dominio derivam desta classe para permitir que o consumidor
/// distinga falhas de regra do negocio de falhas tecnicas do framework.
/// </summary>
public abstract class DominioException : Exception
{
    /// <summary>
    /// Inicializa uma nova instancia de <see cref="DominioException"/>.
    /// </summary>
    protected DominioException()
    {
    }

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="DominioException"/> com a mensagem informada.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro de dominio.</param>
    protected DominioException(string message) : base(message)
    {
    }

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="DominioException"/> com a mensagem e a excecao interna informadas.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro de dominio.</param>
    /// <param name="innerException">A excecao que originou o erro atual.</param>
    protected DominioException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
