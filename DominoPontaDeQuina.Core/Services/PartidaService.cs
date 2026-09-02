using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Core.Services;

public class PartidaService
{
    private readonly IPartidaRepository _partidaRepository;

    public PartidaService(IPartidaRepository partidaRepository)
    {
        _partidaRepository = partidaRepository;
    }

    public async Task<Partida?> BuscarPorIdAsync(Guid id)
    {
        return await _partidaRepository.BuscarPorIdAsync(id);
    }

    public async Task<List<Partida>> BuscarTodosAsync()
    {
        return await _partidaRepository.BuscarTodosAsync();
    }

    public async Task CriarAsync(Partida partida)
    {
        await _partidaRepository.AdicionarAsync(partida);
    }

    public async Task AtualizarAsync(Partida partida)
    {
        await _partidaRepository.AtualizarAsync(partida);
    }

    public async Task RemoverAsync(Partida partida)
    {
        await _partidaRepository.RemoverAsync(partida);
    }
}