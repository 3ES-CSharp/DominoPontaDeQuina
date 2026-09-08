using Microsoft.AspNetCore.Mvc;

namespace DominoPontaDeQuina.Api.Middleware;

/// <summary>Converte exceções da camada de aplicação em respostas ProblemDetails.</summary>
public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    /// <summary>Executa o próximo middleware e trata as exceções conhecidas.</summary>
    /// <param name="context">Contexto da requisição.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var (status, titulo) = exception switch
            {
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso não encontrado."),
                ArgumentException => (StatusCodes.Status400BadRequest, "Requisição inválida."),
                InvalidOperationException => (StatusCodes.Status409Conflict, "Operação não permitida no estado atual."),
                _ => (StatusCodes.Status500InternalServerError, "Erro inesperado.")
            };

            if (status == StatusCodes.Status500InternalServerError)
                logger.LogError(exception, "Erro não tratado ao processar {Path}.", context.Request.Path);

            if (context.Response.HasStarted)
                throw;

            context.Response.Clear();
            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = status,
                Title = titulo,
                Detail = status == StatusCodes.Status500InternalServerError ? null : exception.Message,
                Instance = context.Request.Path
            });
        }
    }
}
