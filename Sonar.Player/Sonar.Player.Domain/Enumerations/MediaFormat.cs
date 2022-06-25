using Sonar.Player.Domain.Tools;
using Sonar.Player.Domain.Tools.Exceptions;

namespace Sonar.Player.Domain.Enumerations;

public class MediaFormat : Enumeration<string, MediaFormat>
{
    protected MediaFormat(string name, string format) 
        : base(name, format) { }

    protected MediaFormat() {}
}