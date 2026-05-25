// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583) - Grupo 3ESPF/05
using System;

namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Exceção lançada ao tentar realizar uma jogada inválida ou incompatível com o tabuleiro.
/// </summary>
public class JogadaInvalidaException : DominoException
{
    public JogadaInvalidaException() : base() { }

    public JogadaInvalidaException(string message) : base(message) { }

    public JogadaInvalidaException(string message, Exception innerException) : base(message, innerException) { }
}
