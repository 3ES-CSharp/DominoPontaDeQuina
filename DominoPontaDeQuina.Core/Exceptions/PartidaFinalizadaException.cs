namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Excecao lancada quando uma operacao incompativel com o estado finalizado da partida e tentada.
/// </summary>
public class PartidaFinalizadaException : DominioException
{
    private const string MensagemPadrao = "A partida ja foi finalizada e nao aceita novas operacoes.";

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="PartidaFinalizadaException"/> com a mensagem padrao.
    /// </summary>
    public PartidaFinalizadaException() : base(MensagemPadrao)
    {
    }

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="PartidaFinalizadaException"/> com a mensagem informada.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    public PartidaFinalizadaException(string message) : base(message)
    {
    }

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="PartidaFinalizadaException"/> com a mensagem e excecao interna informadas.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    /// <param name="innerException">A excecao que originou o erro atual.</param>
    public PartidaFinalizadaException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
