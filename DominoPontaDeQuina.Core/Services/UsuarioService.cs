using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Core.Services;

public class UsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Usuario?> BuscarPorIdAsync(Guid id)
    {
        return await _usuarioRepository.BuscarPorIdAsync(id);
    }

    public async Task<List<Usuario>> BuscarTodosAsync()
    {
        return await _usuarioRepository.BuscarTodosAsync();
    }

    public async Task<List<Usuario>> BuscarPorNomeAsync(string nome)
    {
        return await _usuarioRepository.BuscarPorNomeAsync(nome);
    }

    public async Task CriarAsync(Usuario usuario)
    {
        await _usuarioRepository.AdicionarAsync(usuario);
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        await _usuarioRepository.AtualizarAsync(usuario);
    }

    public async Task RemoverAsync(Usuario usuario)
    {
        await _usuarioRepository.RemoverAsync(usuario);
    }
}