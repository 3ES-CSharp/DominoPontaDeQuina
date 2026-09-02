using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Core.Services;
using DominoPontaDeQuina.Repository.Context;
using DominoPontaDeQuina.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddDbContext<DominoDbContext>(options =>
    options.UseSqlite(
        "Data Source=domino.db",
        sqliteOptions =>
            sqliteOptions.MigrationsAssembly("DominoPontaDeQuina.Migrations")));

services.AddScoped<IUsuarioRepository, UsuarioRepository>();
services.AddScoped<IJogadorRepository, JogadorRepository>();
services.AddScoped<IPartidaRepository, PartidaRepository>();
services.AddScoped<IParticipacaoPartidaRepository, ParticipacaoPartidaRepository>();

services.AddScoped<UsuarioService>();
services.AddScoped<JogadorService>();
services.AddScoped<PartidaService>();
services.AddScoped<ParticipacaoPartidaService>();

var serviceProvider = services.BuildServiceProvider();

Console.WriteLine("Injecao de dependencias configurada com sucesso.");