using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sonar.Player.Application.Tools.Exceptions;
using Sonar.Player.Domain.Tools.Exceptions;
using ILogger = Serilog.ILogger;

namespace Sonar.Player.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger _logger;

    public ExceptionFilter(ILogger logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.Error(context.Exception, "");

        context.Result = new JsonResult(context.Exception.Message)
        {
            StatusCode = context.Exception switch
            {
                ExternalApiException apiException => apiException.StatusCode,
                NotEnoughAccessException => StatusCodes.Status403Forbidden,
                EnumerationParseException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            }
        };
        context.ExceptionHandled = true;
    }
}
