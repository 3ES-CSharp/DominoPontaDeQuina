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
    public DbSet<Jogo> Jogos { get; set; }
}