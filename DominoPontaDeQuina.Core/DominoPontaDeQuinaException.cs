namespace DominoPontaDeQuina.Core;

/// <summary>
/// Excecao de dominio do jogo de domino.
/// Utilizada para sinalizar violacoes de regras de negocio dentro do dominio da aplicacao.
/// </summary>
public class DominoPontaDeQuinaException : Exception
{
    public DominoPontaDeQuinaException(string message) : base(message) { }

    public DominoPontaDeQuinaException(string message, Exception innerException)
        : base(message, innerException) { }
}
