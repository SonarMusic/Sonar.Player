using Sonar.Player.Domain.Tools.Exceptions;

namespace Sonar.Player.Application.Tools.Exceptions;

public class TrackNotFoundException : SonarPlayerException
{
    public TrackNotFoundException(string message) : base(message) { }
}