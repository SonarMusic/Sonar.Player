using Sonar.Player.Domain.Tools.Exceptions;

namespace Sonar.Player.Domain.Enumerations;

public class AudioFormat : MediaFormat
{
    protected AudioFormat()
    { }
    
    protected AudioFormat(string name, string format)
        : base(name, format) { }
    
    public static AudioFormat Mp3 => new AudioFormat("Mp3", "mp3");
    public static AudioFormat Wav => new AudioFormat("Wav", "wav");

    public static AudioFormat FromFileName(string filename)
    {
        ArgumentNullException.ThrowIfNull(filename);
        return Path.GetExtension(filename) switch
        {
            ".mp3" => Mp3,
            ".wav" => Wav,
            _ => throw new EnumerationParseException(nameof(AudioFormat), filename)
        };
    }
}