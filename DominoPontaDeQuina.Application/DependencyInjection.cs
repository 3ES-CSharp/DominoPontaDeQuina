using DominoPontaDeQuina.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DominoPontaDeQuina.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddDominoApplication(
        this IServiceCollection services)
    {
        services.AddScoped<UsuarioService>();
        services.AddScoped<JogadorService>();
        services.AddScoped<PartidaService>();
        services.AddScoped<ParticipacaoPartidaService>();

        return services;
    }
}