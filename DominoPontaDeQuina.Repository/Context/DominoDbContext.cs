using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Context;

public class DominoDbContext : DbContext
{
    public DominoDbContext(DbContextOptions<DominoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Jogador> Jogadores { get; set; }
    public DbSet<Partida> Partidas { get; set; }
    public DbSet<ParticipacaoPartida> ParticipacoesPartida { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Jogadores)
            .WithOne(j => j.Usuario)
            .HasForeignKey(j => j.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ParticipacaoPartida>()
            .HasOne(pp => pp.Jogador)
            .WithMany(j => j.Participacoes)
            .HasForeignKey(pp => pp.JogadorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ParticipacaoPartida>()
            .HasOne(pp => pp.Partida)
            .WithMany(p => p.Participacoes)
            .HasForeignKey(pp => pp.PartidaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}