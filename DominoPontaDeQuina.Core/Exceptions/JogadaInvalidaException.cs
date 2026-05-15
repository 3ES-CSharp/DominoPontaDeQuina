namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Exceção lançada quando um jogador tenta realizar uma jogada que não é permitida pelas regras da rodada ou do tabuleiro.
/// </summary>
public class JogadaInvalidaException(string message) : DominoException(message);