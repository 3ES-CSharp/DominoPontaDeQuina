namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Exceção lançada quando um jogador não possui peças compatíveis com o tabuleiro,
/// mas tenta executar uma jogada (em vez de passar a vez).
/// </summary>
public class SemPecasCompativeisException : DominoException
{
    /// <summary>
    /// Inicializa uma nova instância da exceção com uma mensagem de erro específica.
    /// </summary>
    /// <param name="message">Mensagem indicando que o jogador não tem peças compatíveis.</param>
    public SemPecasCompativeisException(string message) : base(message) { }
}