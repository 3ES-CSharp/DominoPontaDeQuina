using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Contrato para o serviço de gerenciamento de finalização da rodada.
/// </summary>
internal interface IRodadaService
{
    Jogador? VerificarBatida(IEnumerable<MaoJogador> maosJogadores);
    bool VerificarTabuleiroTravado(Tabuleiro tabuleiro, IEnumerable<MaoJogador> maosJogadores, IJogadaValidator jogadorValidator);
    Jogador? DeterminarVencedor(IEnumerable<MaoJogador> maosJogadores, TipoFinalizacaoRodada tipoFinalizacao, Jogador? jogadorQueBateu = null);
}