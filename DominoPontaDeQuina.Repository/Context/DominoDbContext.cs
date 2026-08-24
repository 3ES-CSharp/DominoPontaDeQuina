using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Context;

public class DominoDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Jogador> Jogadores { get; set; }
    public DbSet<Jogo> Jogos { get; set; }
    public DbSet<ParticipacaoJogo> ParticipacoesJogo { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=DominoDB;Trusted_Connection=True;TrustServerCertificate=True;"
        );
    }
}