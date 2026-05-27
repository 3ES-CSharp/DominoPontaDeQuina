// Autoria: Gabriel Galerani (557421) e Leonardo Taschin (554583) - Grupo 3ESPF/05
using System;

namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Exceção lançada ao tentar violar as regras de fluxo e estados da partida de dominó.
/// </summary>
public class PartidaInvalidaException : DominoException
{
    public PartidaInvalidaException() : base() { }

    public PartidaInvalidaException(string message) : base(message) { }

    public PartidaInvalidaException(string message, Exception innerException) : base(message, innerException) { }
}
