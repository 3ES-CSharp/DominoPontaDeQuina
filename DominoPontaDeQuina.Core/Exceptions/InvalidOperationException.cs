using DominoPontaDeQuina.Core.Exceptions;

namespace DominoPontaDeQuina.Core;

/// <summary>
/// Excecao de dominio que sombreia <see cref="System.InvalidOperationException"/> no namespace
/// <c>DominoPontaDeQuina.Core</c>.
/// <para>
/// A classe <see cref="Jogo"/> (imutavel por restricao do projeto) lanca
/// <c>throw new InvalidOperationException(...)</c> em varios pontos do fluxo.
/// O compilador C# resolve esse identificador para este tipo em vez do tipo do <c>System</c>,
/// pois a resolucao de nomes prioriza o namespace declarante. Isso garante que as excecoes
/// lancadas pelo orquestrador do jogo pertencam ao dominio do projeto sem exigir alteracoes
/// no codigo original.
/// </para>
/// </summary>
internal sealed class InvalidOperationException : DominioException
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
    public InvalidOperationException(string message, System.Exception innerException) : base(message, innerException)
    {
    }
}
