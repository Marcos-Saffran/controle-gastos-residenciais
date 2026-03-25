using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Gastos.Api.Middleware;

public sealed class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ApiExceptionMiddleware(
        RequestDelegate next,
        ILogger<ApiExceptionMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception while processing {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteProblemDetailsAsync(context, exception);
        }
    }

    private async Task WriteProblemDetailsAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title) = MapException(exception);

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = ShouldExposeDetails(statusCode) || _environment.IsDevelopment()
                ? exception.Message
                : "Ocorreu um erro interno ao processar a solicitação.",
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }

    private static bool ShouldExposeDetails(HttpStatusCode statusCode)
    {
        return (int)statusCode < 500;
    }

    private static (HttpStatusCode StatusCode, string Title) MapException(Exception exception)
    {
        return exception switch
        {
            ArgumentException => (HttpStatusCode.BadRequest, "Dados inválidos"),
            BadHttpRequestException => (HttpStatusCode.BadRequest, "Requisição inválida"),
            InvalidOperationException invalidOperationException when IsNotFoundMessage(invalidOperationException.Message)
                => (HttpStatusCode.NotFound, "Recurso não encontrado"),
            InvalidOperationException => (HttpStatusCode.BadRequest, "Operação inválida"),
            NotImplementedException => (HttpStatusCode.NotImplemented, "Operação não implementada"),
            _ => (HttpStatusCode.InternalServerError, "Erro interno do servidor")
        };
    }

    private static bool IsNotFoundMessage(string message)
    {
        return message.Contains("não encontrada", StringComparison.OrdinalIgnoreCase)
            || message.Contains("não encontrado", StringComparison.OrdinalIgnoreCase);
    }
}