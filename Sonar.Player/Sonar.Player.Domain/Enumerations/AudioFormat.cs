using System.Text.RegularExpressions;
using Sonar.Player.Domain.Tools;
using Sonar.Player.Domain.Tools.Exceptions;

namespace Sonar.Player.Domain.Enumerations;

public class AudioFormat : Enumeration<string, AudioFormat>
{
    protected AudioFormat(string name, string format) 
        : base(name, format) { }

    protected AudioFormat() : base() {}
    
    public static AudioFormat Mp3 => new AudioFormat("Mp3", "mp3");
    public static AudioFormat Wav => new AudioFormat("Wav", "wav");

    public static AudioFormat FromFileName(string filename)
    {
        ArgumentNullException.ThrowIfNull(filename);
        return Path.GetExtension(filename) switch
        {
            "mp3" => Mp3,
            "wav" => Wav,
            _ => throw new EnumerationParseException<string>(nameof(AudioFormat), filename)
        };
    }
}