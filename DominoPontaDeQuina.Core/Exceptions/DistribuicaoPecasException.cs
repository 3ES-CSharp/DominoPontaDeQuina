namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Excecao lancada quando ocorre uma falha durante a distribuicao das pecas entre os jogadores.
/// Cenarios comuns incluem quantidade insuficiente de pecas no monte ou lista de jogadores invalida.
/// </summary>
public class DistribuicaoPecasException : DominoException
{
    /// <summary>
    /// Inicializa uma nova instancia da excecao com a mensagem informada.
    /// </summary>
    /// <param name="mensagem">A mensagem descritiva do erro de distribuicao.</param>
    public DistribuicaoPecasException(string mensagem) : base(mensagem)
    {
    }
}
