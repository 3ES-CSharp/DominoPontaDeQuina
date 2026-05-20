namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Excecao base para todas as excecoes de dominio do jogo de domino.
/// Centraliza a hierarquia de erros de negocio para que consumidores da API possam
/// capturar qualquer falha relacionada ao dominio com um unico tipo.
/// </summary>
public abstract class DominoException : Exception
{
    /// <summary>
    /// Inicializa uma nova instancia da excecao com a mensagem informada.
    /// </summary>
    /// <param name="mensagem">A mensagem descritiva do erro de dominio.</param>
    protected DominoException(string mensagem) : base(mensagem)
    {
    }

    /// <summary>
    /// Inicializa uma nova instancia da excecao com a mensagem informada e a excecao interna.
    /// </summary>
    /// <param name="mensagem">A mensagem descritiva do erro de dominio.</param>
    /// <param name="excecaoInterna">A excecao que originou esta falha.</param>
    protected DominoException(string mensagem, Exception excecaoInterna) : base(mensagem, excecaoInterna)
    {
    }
}
