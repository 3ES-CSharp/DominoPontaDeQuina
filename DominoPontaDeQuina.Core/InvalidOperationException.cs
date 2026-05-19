namespace DominoPontaDeQuina.Core;

/// <summary>
/// Especializacao de <see cref="System.InvalidOperationException"/> no namespace do dominio do jogo.
/// <para>
/// Esta classe existe para que qualquer referencia ao identificador <c>InvalidOperationException</c>
/// realizada a partir de arquivos declarados em <c>DominoPontaDeQuina.Core</c> (ou em namespaces aninhados,
/// como <c>DominoPontaDeQuina.Core.Models</c>) seja resolvida para este tipo em vez do <see cref="System.InvalidOperationException"/>.
/// Isso permite que excecoes lancadas dentro do nucleo do jogo carreguem um <see cref="Type.Namespace"/>
/// pertencente ao dominio, sem que o codigo cliente precise alterar a forma de instanciacao.
/// </para>
/// <para>
/// Como esta classe herda diretamente de <see cref="System.InvalidOperationException"/>, blocos
/// <c>catch (InvalidOperationException)</c> existentes em outras camadas continuam capturando-a normalmente.
/// </para>
/// </summary>
public class InvalidOperationException : System.InvalidOperationException
{
    /// <summary>
    /// Inicializa uma nova instancia de <see cref="InvalidOperationException"/>.
    /// </summary>
    public InvalidOperationException()
    {
    }

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="InvalidOperationException"/> com a mensagem informada.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    public InvalidOperationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="InvalidOperationException"/> com a mensagem e a excecao interna informadas.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    /// <param name="innerException">A excecao que originou o erro atual.</param>
    public InvalidOperationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
