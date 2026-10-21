using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Core.Services;

public class JogadorService
{
    private readonly IJogadorRepository _jogadorRepository;

    public JogadorService(IJogadorRepository jogadorRepository)
    {
        _jogadorRepository = jogadorRepository;
    }

    public async Task<Jogador?> BuscarPorIdAsync(Guid id)
    {
        return await _jogadorRepository.BuscarPorIdAsync(id);
    }

    public async Task<List<Jogador>> BuscarTodosAsync()
    {
        return await _jogadorRepository.BuscarTodosAsync();
    }

    public async Task<List<Jogador>> BuscarPorNomeAsync(string nome)
    {
        return await _jogadorRepository.BuscarPorNomeAsync(nome);
    }

    public async Task<List<Jogador>> BuscarPorUsuarioAsync(Guid usuarioId)
    {
        return await _jogadorRepository.BuscarPorUsuarioAsync(usuarioId);
    }

    public async Task CriarAsync(Jogador jogador)
    {
        await _jogadorRepository.AdicionarAsync(jogador);
    }

    public async Task AtualizarAsync(Jogador jogador)
    {
        await _jogadorRepository.AtualizarAsync(jogador);
    }

    public async Task RemoverAsync(Jogador jogador)
    {
        await _jogadorRepository.RemoverAsync(jogador);
    }
}