using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Exceptions;

/// <summary>
/// Excecao lancada quando um jogador esperado nao e localizado em uma colecao de maos da rodada.
/// Sinaliza inconsistencia entre quem foi escolhido para iniciar a rodada e os jogadores realmente distribuidos.
/// </summary>
public class JogadorNaoEncontradoException : DominoException
{
    /// <summary>
    /// Obtem o jogador que nao foi encontrado, quando disponivel.
    /// </summary>
    public Jogador? Jogador { get; }

    /// <summary>
    /// Inicializa uma nova instancia da excecao com a mensagem padrao referenciando o jogador ausente.
    /// </summary>
    /// <param name="jogador">O jogador que nao foi encontrado na colecao.</param>
    public JogadorNaoEncontradoException(Jogador jogador)
        : base($"O jogador '{jogador?.Nome ?? "desconhecido"}' nao esta presente na lista de jogadores da rodada.")
    {
        Jogador = jogador;
    }

    /// <summary>
    /// Inicializa uma nova instancia da excecao com uma mensagem personalizada.
    /// </summary>
    /// <param name="mensagem">A mensagem descritiva do erro.</param>
    public JogadorNaoEncontradoException(string mensagem) : base(mensagem)
    {
    }
}
