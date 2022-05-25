namespace Sonar.Player.Domain.Tools.Exceptions;

//TODO: add base domain exception
public class EnumerationParseException<T> : Exception
{
    public EnumerationParseException(string typeName, T value)
        : base($"Can't parse {typeName} from {value}") { }
}