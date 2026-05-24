// Exceptions/PartidaFinalizadaException.cs
namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Lançada ao tentar executar operações em uma partida já finalizada.
/// </summary>
public class PartidaFinalizadaException : DominoException
{
    public PartidaFinalizadaException() { }
    public PartidaFinalizadaException(string message) : base(message) { }
    public PartidaFinalizadaException(string message, Exception inner) : base(message, inner) { }
}