using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Core.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> BuscarPorIdAsync(Guid id);

    Task<List<Usuario>> BuscarTodosAsync();

    Task<List<Usuario>> BuscarPorNomeAsync(string nome);

    Task AdicionarAsync(Usuario usuario);

    Task AtualizarAsync(Usuario usuario);

    Task RemoverAsync(Usuario usuario);
}