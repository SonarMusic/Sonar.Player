namespace Sonar.Player.Domain.Tools.Exceptions;

public class SonarPlayerException : Exception
{
    public SonarPlayerException() { }

    public SonarPlayerException(string message)
        : base(message) { }
}
