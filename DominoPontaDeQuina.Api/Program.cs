using System.Reflection;
using DominoPontaDeQuina.Api.Middleware;
using DominoPontaDeQuina.Application;
using DominoPontaDeQuina.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("A connection string 'DefaultConnection' não foi configurada.");

// Camada de infraestrutura: DbContext + repositórios.
builder.Services.AddDominoInfrastructure(options => options.UseSqlServer(connectionString));

// Camada de aplicação: casos de uso (IPartidaService).
builder.Services.AddDominoApplication();

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Domino Ponta de Quina API",
        Version = "v1",
        Description = "Endpoints de partidas, jogadores, lances, ranking e histórico."
    });

    var xml = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    if (File.Exists(xml))
        options.IncludeXmlComments(xml);
});

var app = builder.Build();

// Traduz exceções da camada de aplicação em respostas HTTP. Deve vir primeiro.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
