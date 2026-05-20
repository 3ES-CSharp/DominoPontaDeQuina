namespace DominoPontaDeQuina.Core;

/// <summary>
/// Excecao lancada quando uma operacao do dominio e executada sobre um estado invalido,
/// como iniciar uma rodada em uma partida finalizada, finalizar uma partida fora de andamento,
/// registrar uma jogada nula ou executar uma jogada sem partida/rodada em andamento.
/// </summary>
/// <remarks>
/// Esta classe reside intencionalmente no namespace raiz <c>DominoPontaDeQuina.Core</c> e
/// compartilha o nome com <see cref="System.InvalidOperationException"/>. Como a resolucao de
/// nomes em C# considera primeiro o namespace corrente (e em seguida os namespaces pai), todos
/// os usos de <c>new InvalidOperationException(...)</c> em classes pertencentes ao namespace
/// <c>DominoPontaDeQuina.Core</c> ou seus filhos sao resolvidos para esta versao do projeto.
/// Isto garante que qualquer falha de operacao do dominio pertenca a um namespace proprio,
/// permitindo identifica-las e captura-las separadamente das excecoes do BCL.
/// A heranca de <see cref="System.InvalidOperationException"/> preserva a compatibilidade com
/// capturas existentes do tipo base.
/// </remarks>
public class InvalidOperationException : System.InvalidOperationException
{
    /// <summary>
    /// Inicializa uma nova instancia da excecao com a mensagem descritiva informada.
    /// </summary>
    /// <param name="mensagem">A mensagem que descreve a operacao invalida.</param>
    public InvalidOperationException(string mensagem) : base(mensagem)
    {
    }

    /// <summary>
    /// Inicializa uma nova instancia da excecao com a mensagem e a excecao interna informadas.
    /// </summary>
    /// <param name="mensagem">A mensagem que descreve a operacao invalida.</param>
    /// <param name="excecaoInterna">A excecao que originou esta falha.</param>
    public InvalidOperationException(string mensagem, Exception excecaoInterna) : base(mensagem, excecaoInterna)
    {
    }
}
