namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Exceção base do domínio do jogo Dominó Ponta de Quina.
/// </summary>
public class DominoException : Exception
{
    public DominoException() { }
    public DominoException(string message) : base(message) { }
    public DominoException(string message, Exception inner) : base(message, inner) { }
}