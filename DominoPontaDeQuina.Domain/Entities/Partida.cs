namespace DominoPontaDeQuina.Domain.Entities;

// Sem Data Annotations: chave, tabela e tipos saem das convencoes do EF Core.
// Os relacionamentos sao mapeados por Fluent API no DominoDbContext.
public class Partida
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime IniciadoEm { get; set; } = DateTime.UtcNow;
    public DateTime? FinalizadoEm { get; set; }
    public StatusPartida Status { get; set; } = StatusPartida.Aguardando;
    public ICollection<ParticipacaoPartida> Participacoes { get; set; } = new List<ParticipacaoPartida>();
}
