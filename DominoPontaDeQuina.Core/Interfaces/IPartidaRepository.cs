using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Core.Interfaces;

public interface IPartidaRepository
{
    Task<Partida?> BuscarPorIdAsync(Guid id);

    Task<List<Partida>> BuscarTodosAsync();

    Task AdicionarAsync(Partida partida);

    Task AtualizarAsync(Partida partida);

    Task RemoverAsync(Partida partida);
}