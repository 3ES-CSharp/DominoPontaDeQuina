using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Context;

public class DominoDbContext : DbContext
{
    public const string ConexaoPadrao = "Data Source=domino.db";

    public DominoDbContext()
    {
    }

    public DominoDbContext(DbContextOptions<DominoDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Jogador> Jogadores => Set<Jogador>();
    public DbSet<Partida> Partidas => Set<Partida>();
    public DbSet<ParticipacaoPartida> Participacoes => Set<ParticipacaoPartida>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite(ConexaoPadrao);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Usuario e Jogador ja trazem colunas/tabelas por Data Annotations.
        // Aqui ficam apenas os relacionamentos e restricoes de integridade (Fluent API).
        modelBuilder.Entity<Usuario>(usuario =>
        {
            usuario.HasIndex(u => u.Email).IsUnique();

            usuario.HasMany(u => u.Jogadores)
                   .WithOne(j => j.Usuario)
                   .HasForeignKey(j => j.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Jogador>(jogador =>
        {
            jogador.HasIndex(j => new { j.UsuarioId, j.NomeExibicao }).IsUnique();

            jogador.HasMany(j => j.Participacoes)
                   .WithOne(p => p.Jogador)
                   .HasForeignKey(p => p.JogadorId)
                   .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Partida>(partida =>
        {
            partida.Property(p => p.Status)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            partida.HasMany(p => p.Participacoes)
                   .WithOne(x => x.Partida)
                   .HasForeignKey(x => x.PartidaId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ParticipacaoPartida>(participacao =>
        {
            // Um jogador entra uma unica vez por partida e cada posicao e exclusiva.
            participacao.HasIndex(p => new { p.PartidaId, p.JogadorId }).IsUnique();
            participacao.HasIndex(p => new { p.PartidaId, p.Posicao }).IsUnique();
        });
    }
}
