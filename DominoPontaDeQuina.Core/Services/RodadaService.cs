using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;

namespace DominoPontaDeQuina.Core.Services;

/// <summary>
/// Serviço responsável por gerenciar a finalização da rodada (batida ou travamento).
/// </summary>
internal class RodadaService : IRodadaService
{
    private readonly IPlacarService _placarService;

    public RodadaService(IPlacarService placarService)
    {
        _placarService = placarService;
    }

    /// <summary>
    /// Verifica se algum jogador bateu (ficou sem peças).
    /// </summary>
    public Jogador? VerificarBatida(IEnumerable<MaoJogador> maosJogadores) =>
        maosJogadores.FirstOrDefault(mao => mao.EstaSemPecas())?.Jogador;

    /// <summary>
    /// Verifica se o tabuleiro está travado.
    /// </summary>
    public bool VerificarTabuleiroTravado(Tabuleiro tabuleiro, IEnumerable<MaoJogador> maosJogadores, IJogadaValidator validator)
    {
        if (tabuleiro.EstaVazio) return false;
        return !maosJogadores.Any(mao => validator.PossuiPecaCompativel(mao, tabuleiro));
    }

    /// <summary>
    /// Determina o vencedor da rodada (batida = quem bateu; travamento = menor soma de peças).
    /// </summary>
    public Jogador? DeterminarVencedor(IEnumerable<MaoJogador> maosJogadores, TipoFinalizacaoRodada tipo, Jogador? bateu = null)
    {
        if (tipo == TipoFinalizacaoRodada.JogadorBateu)
            return bateu;

        return maosJogadores
            .Where(m => !m.EstaSemPecas())
            .OrderBy(m => _placarService.CalcularPontuacaoMao(m))
            .FirstOrDefault()?.Jogador;
    }
}