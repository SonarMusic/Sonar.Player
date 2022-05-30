using Sonar.Player.Application.Tools.Exceptions;
using Sonar.Player.Domain.Tools.Exceptions;
using ILogger = Serilog.ILogger;

namespace Sonar.Player.Api.Middlewares;

public class ApplicationExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ApplicationExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, ILogger logger)
    {
        try
        {
            await _next(httpContext);
        }
        catch (SonarPlayerException exception)
        {
            logger.Error(exception, "");

            httpContext.Response.StatusCode = exception switch
            {
                // TODO: catch EnumerationParseException in some form or another
                ExternalApiException apiException => apiException.StatusCode,
                NotEnoughAccessException => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            await httpContext.Response.WriteAsJsonAsync(new { Message = "Something went wrong" });
        }
    }
}
