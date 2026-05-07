namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Exceção lançada quando uma jogada viola as regras do jogo de dominó.
/// </summary>
/// <remarks>
/// Cenários de uso:
/// - Jogador tenta colar uma peça em um lado incompatível do tabuleiro
/// - Jogador não possui a peça que está tentando jogar
/// - Jogador tenta passar a vez quando possui peças compatíveis
/// </remarks>
public class JogadaInvalidaException : DominoException
{
    /// <summary>
    /// Inicializa uma nova instância da exceção com uma mensagem de erro específica.
    /// </summary>
    /// <param name="message">Mensagem descrevendo porque a jogada é inválida.</param>
    public JogadaInvalidaException(string message) : base(message) { }

    /// <summary>
    /// Inicializa uma nova instância da exceção com uma mensagem de erro e uma exceção interna.
    /// </summary>
    /// <param name="message">Mensagem que descreve o erro ocorrido.</param>
    /// <param name="innerException">Exceção interna que causou esta exceção.</param>
    public JogadaInvalidaException(string message, Exception innerException) : base(message, innerException) { }
}