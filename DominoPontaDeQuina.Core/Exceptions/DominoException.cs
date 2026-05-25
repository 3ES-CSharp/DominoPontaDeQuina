// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583) - Grupo 3ESPF/05
using System;

namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Exceção base do domínio do jogo de dominó Ponta de Quina.
/// </summary>
public class DominoException : Exception
{
    public DominoException() : base() { }

    public DominoException(string message) : base(message) { }

    public DominoException(string message, Exception innerException) : base(message, innerException) { }
}
