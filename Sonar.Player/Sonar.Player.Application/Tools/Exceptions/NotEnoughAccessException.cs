using Sonar.Player.Domain.Tools.Exceptions;

namespace Sonar.Player.Application.Tools.Exceptions;

public class NotEnoughAccessException : SonarPlayerException
{
    public NotEnoughAccessException() { }
    public NotEnoughAccessException(string message) : base(message) { }
}