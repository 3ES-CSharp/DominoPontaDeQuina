using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Core.Interfaces;

public interface IJogadorRepository
{
    Task<Jogador?> BuscarPorIdAsync(Guid id);

    Task<List<Jogador>> BuscarTodosAsync();

    Task<List<Jogador>> BuscarPorNomeAsync(string nome);

    Task<List<Jogador>> BuscarPorUsuarioAsync(Guid usuarioId);

    Task AdicionarAsync(Jogador jogador);

    Task AtualizarAsync(Jogador jogador);

    Task RemoverAsync(Jogador jogador);
}