using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Contrato para o serviço de distribuição de peças do dominó.
/// </summary>
internal interface IDistribuicaoService
{
    /// <summary>
    /// Distribui as 28 peças do dominó entre os jogadores participantes.
    /// </summary>
    /// <param name="jogadores">Lista de jogadores que receberão as peças.</param>
    /// <returns>Lista de objetos MaoJogador contendo as peças distribuídas para cada jogador.</returns>
    List<MaoJogador> DistribuirPecas(IReadOnlyList<Jogador> jogadores);
}