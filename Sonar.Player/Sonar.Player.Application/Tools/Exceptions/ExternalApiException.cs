namespace Sonar.Player.Application.Tools.Exceptions;

public class ExternalApiException : Exception
{
    public int StatusCode { get; }

    public ExternalApiException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}