namespace Sonar.Player.Domain.Tools.Exceptions;

public class EnumerationParseException : SonarPlayerException
{
    public EnumerationParseException(string typeName, string value)
        : base($"Can't parse {typeName} from {value}") { }
}