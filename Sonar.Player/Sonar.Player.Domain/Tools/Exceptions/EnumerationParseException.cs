namespace Sonar.Player.Domain.Tools.Exceptions;

public class EnumerationParseException<T> : SonarPlayerException
{
    public EnumerationParseException(string typeName, T value)
        : base($"Can't parse {typeName} from {value}") { }
}