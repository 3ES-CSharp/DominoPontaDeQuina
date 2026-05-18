namespace DominoPontaDeQuina.Core;

/// <summary>
/// Exceção de operação inválida do domínio do projeto.
/// </summary>
public class InvalidOperationException : Exception
{
    /// <summary>
    /// Inicializa uma nova instância com a mensagem informada.
    /// </summary>
    public InvalidOperationException(string message) : base(message)
    {
    }
}