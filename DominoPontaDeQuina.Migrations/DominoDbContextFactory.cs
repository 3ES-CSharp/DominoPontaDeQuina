using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DominoPontaDeQuina.Migrations;

public class DominoDbContextFactory : IDesignTimeDbContextFactory<DominoDbContext>
{
    public DominoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DominoDbContext>();

        // Usando SQLite como banco de dados
        optionsBuilder.UseSqlite(
            "Data Source=domino.db",
            b => b.MigrationsAssembly("DominoPontaDeQuina.Migrations"));

        return new DominoDbContext(optionsBuilder.Options);
    }
}
