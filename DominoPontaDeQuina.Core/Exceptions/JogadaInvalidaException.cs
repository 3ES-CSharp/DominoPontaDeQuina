namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Lançada quando uma jogada viola as regras do jogo.
/// </summary>
public class JogadaInvalidaException : DominoException
{
    public JogadaInvalidaException() { }
    public JogadaInvalidaException(string message) : base(message) { }
    public JogadaInvalidaException(string message, Exception inner) : base(message, inner) { }
}