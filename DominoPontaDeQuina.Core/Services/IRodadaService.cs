using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Contrato para o serviço de gerenciamento de finalização da rodada.
/// </summary>
internal interface IRodadaService
{
    /// <summary>
    /// Verifica se algum jogador bateu (ficou sem peças na mão).
    /// </summary>
    Jogador? VerificarBatida(IEnumerable<MaoJogador> maosJogadores);

    /// <summary>
    /// Verifica se o tabuleiro está travado (nenhum jogador tem peças compatíveis).
    /// </summary>
    bool VerificarTabuleiroTravado(Tabuleiro tabuleiro, IEnumerable<MaoJogador> maosJogadores, IJogadaValidator jogadorValidator);

    /// <summary>
    /// Determina o jogador vencedor da rodada baseado no tipo de finalização.
    /// </summary>
    Jogador? DeterminarVencedor(IEnumerable<MaoJogador> maosJogadores, TipoFinalizacaoRodada tipoFinalizacao, Jogador? jogadorQueBateu = null);
}