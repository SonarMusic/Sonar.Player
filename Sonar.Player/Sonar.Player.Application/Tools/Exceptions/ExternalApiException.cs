using Sonar.Player.Domain.Tools.Exceptions;

namespace Sonar.Player.Application.Tools.Exceptions;

public class ExternalApiException : SonarPlayerException
{
    public int StatusCode { get; }

    public ExternalApiException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}