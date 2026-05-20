namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Excecao lancada quando a rodada esta em um estado inconsistente para a operacao solicitada.
/// Cenarios incluem consultar o vencedor de uma rodada que nao terminou, registrar jogadas em uma
/// rodada finalizada, ou organizar jogadores quando a fila ja esta preenchida.
/// </summary>
public class RodadaInvalidaException : DominoException
{
    /// <summary>
    /// Inicializa uma nova instancia da excecao com a mensagem informada.
    /// </summary>
    /// <param name="mensagem">A mensagem descritiva do erro de rodada.</param>
    public RodadaInvalidaException(string mensagem) : base(mensagem)
    {
    }
}
