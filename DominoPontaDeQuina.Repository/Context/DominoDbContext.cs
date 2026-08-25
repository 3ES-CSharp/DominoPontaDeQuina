using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Context;

public class DominoDbContext : DbContext
{
    public DominoDbContext(DbContextOptions<DominoDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Jogador> Jogadores => Set<Jogador>();
    public DbSet<Partida> Partidas => Set<Partida>();
    public DbSet<ParticipacaoPartida> ParticipacoesPartida => Set<ParticipacaoPartida>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurarUsuario(modelBuilder);
        ConfigurarJogador(modelBuilder);
        ConfigurarPartida(modelBuilder);
        ConfigurarParticipacaoPartida(modelBuilder);
    }

    // ─────────────────────────────────────────────
    // Usuario
    // ─────────────────────────────────────────────
    private static void ConfigurarUsuario(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(180);

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.HashSenha)
                .IsRequired();

            entity.Property(u => u.CriadoEm)
                .IsRequired();

            // 1 Usuario -> N Jogadores
            entity.HasMany(u => u.Jogadores)
                .WithOne(j => j.Usuario)
                .HasForeignKey(j => j.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    // ─────────────────────────────────────────────
    // Jogador
    // ─────────────────────────────────────────────
    private static void ConfigurarJogador(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Jogador>(entity =>
        {
            entity.ToTable("Jogadores");

            entity.HasKey(j => j.Id);

            entity.Property(j => j.NomeExibicao)
                .IsRequired()
                .HasMaxLength(60);

            // 1 Jogador -> N ParticipacoesPartida
            entity.HasMany(j => j.Participacoes)
                .WithOne(p => p.Jogador)
                .HasForeignKey(p => p.JogadorId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    // ─────────────────────────────────────────────
    // Partida
    // ─────────────────────────────────────────────
    private static void ConfigurarPartida(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Partida>(entity =>
        {
            entity.ToTable("Partidas");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.IniciadoEm)
                .IsRequired();

            entity.Property(p => p.FinalizadoEm)
                .IsRequired(false);

            // Enum armazenado como texto legível no banco
            entity.Property(p => p.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            // 1 Partida -> N ParticipacoesPartida
            entity.HasMany(p => p.Participacoes)
                .WithOne(pp => pp.Partida)
                .HasForeignKey(pp => pp.PartidaId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    // ─────────────────────────────────────────────
    // ParticipacaoPartida
    // ─────────────────────────────────────────────
    private static void ConfigurarParticipacaoPartida(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParticipacaoPartida>(entity =>
        {
            entity.ToTable("ParticipacoesPartida");

            entity.HasKey(pp => pp.Id);

            entity.Property(pp => pp.Posicao)
                .IsRequired();

            entity.Property(pp => pp.Pontuacao)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(pp => pp.Vencedor)
                .IsRequired();

            // Um jogador so pode ter uma participacao por partida
            entity.HasIndex(pp => new { pp.PartidaId, pp.JogadorId })
                .IsUnique();
        });
    }
}
