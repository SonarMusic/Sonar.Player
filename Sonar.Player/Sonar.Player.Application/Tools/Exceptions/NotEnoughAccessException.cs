namespace Sonar.Player.Application.Tools.Exceptions;

public class NotEnoughAccessException : Exception
{
    public NotEnoughAccessException() { }
    public NotEnoughAccessException(string message) : base(message) { }
}