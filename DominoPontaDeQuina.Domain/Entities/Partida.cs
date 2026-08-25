namespace DominoPontaDeQuina.Domain.Entities;

/// <summary>
/// Representa uma partida do jogo de dominó.
/// O mapeamento completo desta entidade é feito via Fluent API no DominoDbContext.
/// </summary>
public class Partida
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime IniciadoEm { get; set; } = DateTime.UtcNow;
    public DateTime? FinalizadoEm { get; set; }
    public StatusPartida Status { get; set; } = StatusPartida.Aguardando;
    public ICollection<ParticipacaoPartida> Participacoes { get; set; } = new List<ParticipacaoPartida>();
}
