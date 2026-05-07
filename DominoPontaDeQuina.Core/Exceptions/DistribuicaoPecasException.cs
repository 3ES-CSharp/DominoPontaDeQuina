namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Exceção lançada quando ocorre um erro durante a distribuição inicial das peças do dominó.
/// </summary>
/// <remarks>
/// Cenários que podem disparar esta exceção:
/// - Número de jogadores diferente de 2 ou 4 (não suportado)
/// - Lista de jogadores vazia ou nula
/// - Erro na geração das 28 peças do dominó
/// </remarks>
public class DistribuicaoPecasException : DominoException
{
    /// <summary>
    /// Inicializa uma nova instância da exceção com uma mensagem de erro específica.
    /// </summary>
    /// <param name="message">Mensagem descrevendo o erro na distribuição.</param>
    public DistribuicaoPecasException(string message) : base(message) { }

    /// <summary>
    /// Inicializa uma nova instância da exceção com uma mensagem de erro e uma exceção interna.
    /// </summary>
    /// <param name="message">Mensagem que descreve o erro ocorrido.</param>
    /// <param name="innerException">Exceção interna que causou esta exceção.</param>
    public DistribuicaoPecasException(string message, Exception innerException) : base(message, innerException) { }
}