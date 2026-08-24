using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Context;

public class DominoDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<Jogador> Jogadores { get; set; }

    public DbSet<Partida> Partidas { get; set; }

    public DbSet<ParticipacaoPartida> ParticipacoesPartida { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=DominoDB;Trusted_Connection=True;TrustServerCertificate=True;"
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Usuario 1:N Jogadores
        modelBuilder.Entity<Usuario>()
            .HasMany(usuario => usuario.Jogadores)
            .WithOne(jogador => jogador.Usuario)
            .HasForeignKey(jogador => jogador.UsuarioId);

        // Jogador 1:N Participacoes
        modelBuilder.Entity<Jogador>()
            .HasMany(jogador => jogador.Participacoes)
            .WithOne(participacao => participacao.Jogador)
            .HasForeignKey(participacao => participacao.JogadorId);

        // Partida 1:N Participacoes
        modelBuilder.Entity<Partida>()
            .HasMany(partida => partida.Participacoes)
            .WithOne(participacao => participacao.Partida)
            .HasForeignKey(participacao => participacao.PartidaId);
    }
}