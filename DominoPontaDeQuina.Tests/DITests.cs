using DominoPontaDeQuina.Core.Services;
using DominoPontaDeQuina.Repository.Context;
using DominoPontaDeQuina.Core.Interfaces;
using DominoPontaDeQuina.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DominoPontaDeQuina.Tests;

public class DITests
{
    private ServiceProvider CriarServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddDbContext<DominoDbContext>(options =>
            options.UseSqlite("Data Source=teste_domino.db"));

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IJogadorRepository, JogadorRepository>();
        services.AddScoped<IPartidaRepository, PartidaRepository>();
        services.AddScoped<IParticipacaoPartidaRepository, ParticipacaoPartidaRepository>();

        services.AddScoped<UsuarioService>();
        services.AddScoped<JogadorService>();
        services.AddScoped<PartidaService>();
        services.AddScoped<ParticipacaoPartidaService>();

        return services.BuildServiceProvider();
    }

    [Fact]
    public void DeveResolverUsuarioService()
    {
        using var provider = CriarServiceProvider();

        var service = provider.GetRequiredService<UsuarioService>();

        Assert.NotNull(service);
    }

    [Fact]
    public void DeveResolverJogadorService()
    {
        using var provider = CriarServiceProvider();

        var service = provider.GetRequiredService<JogadorService>();

        Assert.NotNull(service);
    }

    [Fact]
    public void DeveResolverPartidaService()
    {
        using var provider = CriarServiceProvider();

        var service = provider.GetRequiredService<PartidaService>();

        Assert.NotNull(service);
    }

    [Fact]
    public void DeveResolverParticipacaoPartidaService()
    {
        using var provider = CriarServiceProvider();

        var service = provider.GetRequiredService<ParticipacaoPartidaService>();

        Assert.NotNull(service);
    }
}