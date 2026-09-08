using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Core.Services;

public class ParticipacaoPartidaService
{
    private readonly IParticipacaoPartidaRepository _participacaoRepository;

    public ParticipacaoPartidaService(
        IParticipacaoPartidaRepository participacaoRepository)
    {
        _participacaoRepository = participacaoRepository;
    }

    public async Task<ParticipacaoPartida?> BuscarPorIdAsync(Guid id)
    {
        return await _participacaoRepository.BuscarPorIdAsync(id);
    }

    public async Task<List<ParticipacaoPartida>> BuscarTodosAsync()
    {
        return await _participacaoRepository.BuscarTodosAsync();
    }

    public async Task<List<ParticipacaoPartida>> BuscarPorPartidaAsync(Guid partidaId)
    {
        return await _participacaoRepository.BuscarPorPartidaAsync(partidaId);
    }

    public async Task<List<ParticipacaoPartida>> BuscarPorJogadorAsync(Guid jogadorId)
    {
        return await _participacaoRepository.BuscarPorJogadorAsync(jogadorId);
    }

    public async Task CriarAsync(ParticipacaoPartida participacao)
    {
        await _participacaoRepository.AdicionarAsync(participacao);
    }

    public async Task AtualizarAsync(ParticipacaoPartida participacao)
    {
        await _participacaoRepository.AtualizarAsync(participacao);
    }

    public async Task RemoverAsync(ParticipacaoPartida participacao)
    {
        await _participacaoRepository.RemoverAsync(participacao);
    }
}