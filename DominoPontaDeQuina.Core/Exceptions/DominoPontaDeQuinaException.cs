namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Exceção base do domínio do projeto.
/// </summary>
public class DominoPontaDeQuinaException : Exception
{
    /// <summary>
    /// Inicializa uma nova instância da exceção com a mensagem informada.
    /// </summary>
    public DominoPontaDeQuinaException(string message) : base(message)
    {
    }
}