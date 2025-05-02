using System.Net;
using System.Text.Json;

namespace Chirp.API.Filters;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            object payload = _env.IsDevelopment()
                ? new { title = ex.Message, stackTrace = ex.StackTrace }
                : new { title = "An unexpected error occurred." };

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}