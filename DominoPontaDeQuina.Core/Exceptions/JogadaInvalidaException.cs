namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Excecao lancada quando uma jogada nao pode ser aplicada ao tabuleiro segundo as regras do dominio.
/// Ocorre tipicamente em tentativas de colar uma peca incompativel com a ponta escolhida.
/// </summary>
public class JogadaInvalidaException : DominioException
{
    private const string MensagemPadrao = "A jogada e invalida segundo as regras do dominio.";

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="JogadaInvalidaException"/> com a mensagem padrao.
    /// </summary>
    public JogadaInvalidaException() : base(MensagemPadrao)
    {
    }

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="JogadaInvalidaException"/> com a mensagem informada.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    public JogadaInvalidaException(string message) : base(message)
    {
    }

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="JogadaInvalidaException"/> com a mensagem e excecao interna informadas.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    /// <param name="innerException">A excecao que originou o erro atual.</param>
    public JogadaInvalidaException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
