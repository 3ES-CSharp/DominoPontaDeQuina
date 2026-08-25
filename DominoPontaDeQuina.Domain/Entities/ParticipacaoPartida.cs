namespace DominoPontaDeQuina.Domain.Entities;

/// <summary>
/// Representa a participação de um jogador em uma partida de dominó.
/// O mapeamento completo desta entidade é feito via Fluent API no DominoDbContext.
/// </summary>
public class ParticipacaoPartida
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PartidaId { get; set; }
    public Partida Partida { get; set; } = null!;
    public Guid JogadorId { get; set; }
    public Jogador Jogador { get; set; } = null!;
    public int Posicao { get; set; }
    public int Pontuacao { get; set; }
    public bool Vencedor { get; set; }
}
