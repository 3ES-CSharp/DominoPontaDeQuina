namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Lançada ao tentar acessar estado da rodada antes da inicialização.
/// </summary>
public class RodadaNaoIniciadaException : DominoException
{
    public RodadaNaoIniciadaException() { }
    public RodadaNaoIniciadaException(string message) : base(message) { }
    public RodadaNaoIniciadaException(string message, Exception inner) : base(message, inner) { }
}