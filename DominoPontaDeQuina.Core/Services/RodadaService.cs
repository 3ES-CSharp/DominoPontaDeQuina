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

    public Jogador? VerificarBatida(IEnumerable<MaoJogador> maosJogadores) =>
        maosJogadores.FirstOrDefault(mao => mao.EstaSemPecas())?.Jogador;

    public bool VerificarTabuleiroTravado(Tabuleiro tabuleiro, IEnumerable<MaoJogador> maosJogadores, IJogadaValidator validator)
    {
        if (tabuleiro.EstaVazio) return false;
        return !maosJogadores.Any(mao => validator.PossuiPecaCompativel(mao, tabuleiro));
    }

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