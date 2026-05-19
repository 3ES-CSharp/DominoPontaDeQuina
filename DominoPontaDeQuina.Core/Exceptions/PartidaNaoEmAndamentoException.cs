namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Excecao lancada quando uma operacao exige uma partida em andamento mas o estado atual nao satisfaz essa condicao.
/// </summary>
public class PartidaNaoEmAndamentoException : DominioException
{
    private const string MensagemPadrao = "A partida nao esta em andamento.";

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="PartidaNaoEmAndamentoException"/> com a mensagem padrao.
    /// </summary>
    public PartidaNaoEmAndamentoException() : base(MensagemPadrao)
    {
    }

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="PartidaNaoEmAndamentoException"/> com a mensagem informada.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    public PartidaNaoEmAndamentoException(string message) : base(message)
    {
    }

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="PartidaNaoEmAndamentoException"/> com a mensagem e excecao interna informadas.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    /// <param name="innerException">A excecao que originou o erro atual.</param>
    public PartidaNaoEmAndamentoException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
