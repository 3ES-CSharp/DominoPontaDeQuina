using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DominoPontaDeQuina.Migrations;

public class DominoDbContextFactory : IDesignTimeDbContextFactory<DominoDbContext>
{
    public DominoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DominoDbContext>();

        optionsBuilder.UseSqlite(
            "Data Source=domino.db",
            options => options.MigrationsAssembly("DominoPontaDeQuina.Migrations"));

        return new DominoDbContext(optionsBuilder.Options);
    }
}
