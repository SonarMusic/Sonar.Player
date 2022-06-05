using Sonar.Player.Domain.Tools.Exceptions;

namespace Sonar.Player.Application.Tools.Exceptions;

public class NotFoundException : SonarPlayerException
{
    public NotFoundException(string message) : base(message) { }
}