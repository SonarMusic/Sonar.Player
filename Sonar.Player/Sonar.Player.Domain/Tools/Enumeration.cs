namespace Sonar.Player.Domain.Tools;

public abstract class Enumeration<TValue, TEnumeration> : IEquatable<TEnumeration>
    where TEnumeration : Enumeration<TValue, TEnumeration>
{
    protected Enumeration(string name, TValue value)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
        Value = value;
    }

    protected Enumeration() { }

    public string Name { get; private init; }
    public TValue Value { get; private init; }

    public static bool operator ==(Enumeration<TValue, TEnumeration> left, Enumeration<TValue, TEnumeration> right)
    {
        if ((left, right) is (null, null))
            return true;

        return left?.Equals(right) ?? false;
    }

    public static bool operator !=(Enumeration<TValue, TEnumeration> left, Enumeration<TValue, TEnumeration> right)
        => !(left == right);

    public bool Equals(TEnumeration? other)
        => other?.Value?.Equals(Value) ?? false;

    public override bool Equals(object? obj)
        => Equals(obj as TEnumeration);

    public override int GetHashCode()
        => Value?.GetHashCode() ?? 0;

    public override string ToString()
        => Name;
}