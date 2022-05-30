using ILogger = Serilog.ILogger;

namespace Sonar.Player.Api.Middlewares;

public class DefaultExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public DefaultExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, ILogger logger)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            logger.Error(exception, "");

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new { Message = "Internal server error" });
        }
    }
}
