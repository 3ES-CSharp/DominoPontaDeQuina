using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Excecao lancada quando uma jogada nao pode ser aplicada ao tabuleiro.
/// Inclui cenarios como tentar colar uma peca em uma ponta cujo valor nao e compativel
/// ou quando a regra de domino impede a execucao da jogada solicitada.
/// </summary>
public class JogadaInvalidaException : DominoException
{
    /// <summary>
    /// Obtem a peca envolvida na jogada invalida, quando disponivel.
    /// </summary>
    public Peca? Peca { get; }

    /// <summary>
    /// Obtem o lado do tabuleiro em que foi tentada a jogada, quando disponivel.
    /// </summary>
    public LadoTabuleiro? Lado { get; }

    /// <summary>
    /// Inicializa uma nova instancia da excecao com detalhes da peca e do lado tentados.
    /// </summary>
    /// <param name="peca">A peca que nao pode ser colada.</param>
    /// <param name="lado">O lado do tabuleiro em que a tentativa ocorreu.</param>
    public JogadaInvalidaException(Peca peca, LadoTabuleiro lado)
        : base($"A peca {peca} nao pode ser colada no lado {lado} do tabuleiro.")
    {
        Peca = peca;
        Lado = lado;
    }

    /// <summary>
    /// Inicializa uma nova instancia da excecao com uma mensagem personalizada.
    /// </summary>
    /// <param name="mensagem">A mensagem descritiva do erro.</param>
    public JogadaInvalidaException(string mensagem) : base(mensagem)
    {
    }
}
