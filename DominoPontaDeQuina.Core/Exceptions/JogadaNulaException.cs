namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Excecao lancada quando uma jogada nula e submetida ao fluxo da rodada.
/// Indica violacao do contrato esperado: toda jogada registrada deve ser uma instancia valida.
/// </summary>
public class JogadaNulaException : DominioException
{
    private const string MensagemPadrao = "A jogada informada e nula e nao pode ser registrada na rodada.";

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="JogadaNulaException"/> com a mensagem padrao.
    /// </summary>
    public JogadaNulaException() : base(MensagemPadrao)
    {
    }

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="JogadaNulaException"/> com a mensagem informada.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    public JogadaNulaException(string message) : base(message)
    {
    }

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="JogadaNulaException"/> com a mensagem e excecao interna informadas.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    /// <param name="innerException">A excecao que originou o erro atual.</param>
    public JogadaNulaException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
