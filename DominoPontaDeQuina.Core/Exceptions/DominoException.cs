namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Exceção base para regras de domínio violadas no jogo de Dominó Ponta de Quina.
/// </summary>
public class DominoException(string message) : Exception(message);