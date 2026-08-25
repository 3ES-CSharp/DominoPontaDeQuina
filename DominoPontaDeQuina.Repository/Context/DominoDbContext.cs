using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Context;

public class DominoDbContext : DbContext
{
    public DominoDbContext(DbContextOptions<DominoDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Jogador> Jogadores { get; set; }
    public DbSet<Partida> Partidas { get; set; }
    public DbSet<ParticipacaoPartida> ParticipacoesPartida { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurarUsuario(modelBuilder);
        ConfigurarJogador(modelBuilder);
        ConfigurarPartida(modelBuilder);
        ConfigurarParticipacaoPartida(modelBuilder);
    }

    // ─────────────────────────────────────────────
    // Usuário
    // ─────────────────────────────────────────────
    private static void ConfigurarUsuario(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.HashSenha)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(u => u.CriadoEm)
                .IsRequired();

            // 1 Usuario → N Jogadores
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
                .HasMaxLength(50);

            // 1 Jogador → N ParticipacoesPartida
            entity.HasMany(j => j.Participacoes)
                .WithOne(p => p.Jogador)
                .HasForeignKey(p => p.JogadorId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    // ─────────────────────────────────────────────
    // Partida  (convenções + Fluent API para Status)
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

            // Enum armazenado como string legível no banco
            entity.Property(p => p.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            // 1 Partida → N ParticipacoesPartida
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

            // Relacionamentos já configurados no lado HasMany dos pais
            // Declarados aqui apenas para clareza da FK
            entity.HasOne(pp => pp.Partida)
                .WithMany(p => p.Participacoes)
                .HasForeignKey(pp => pp.PartidaId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pp => pp.Jogador)
                .WithMany(j => j.Participacoes)
                .HasForeignKey(pp => pp.JogadorId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
