using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Core.Interfaces;

public interface IParticipacaoPartidaRepository
{
    Task<ParticipacaoPartida?> BuscarPorIdAsync(Guid id);

    Task<List<ParticipacaoPartida>> BuscarTodosAsync();

    Task<List<ParticipacaoPartida>> BuscarPorPartidaAsync(Guid partidaId);

    Task<List<ParticipacaoPartida>> BuscarPorJogadorAsync(Guid jogadorId);

    Task AdicionarAsync(ParticipacaoPartida participacao);

    Task AtualizarAsync(ParticipacaoPartida participacao);

    Task RemoverAsync(ParticipacaoPartida participacao);
}