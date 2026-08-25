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
    public DbSet<Jogo> Jogos { get; set; }
    public DbSet<ParticipacaoJogo> ParticipacaosJogo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Jogo: deixado para as convenções do EF Core
        // O EF Core infere PK (Id), relações de navegação e tipos por convenção

        // Conversão do enum StatusJogo para string no banco de dados
        modelBuilder.Entity<Jogo>()
            .Property(j => j.Status)
            .HasConversion<string>();
    }
}
