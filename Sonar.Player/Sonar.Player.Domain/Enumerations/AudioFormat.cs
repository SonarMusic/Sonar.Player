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

    public static AudioFormat Aac => new AudioFormat("Aac", "aac");
    
    public static AudioFormat Ogg => new AudioFormat("Ogg", "ogg");
    
    public static AudioFormat Aa => new AudioFormat("Aa", "aa");

    public static AudioFormat Aiff => new AudioFormat("Aiff", "aiff");

    public static AudioFormat Ac3 => new AudioFormat("Ac3", "ac3");
    
    public static AudioFormat Adx => new AudioFormat("Adx", "adx");
    
    public static AudioFormat Wma => new AudioFormat("Wma", "wma");

    public static AudioFormat FromFileName(string filename)
    {
        ArgumentNullException.ThrowIfNull(filename);
        return Path.GetExtension(filename) switch
        {
            ".mp3" => Mp3,
            ".wav" => Wav,
            ".aac" => Aac,
            ".ogg" => Ogg,
            ".aa" => Aa,
            ".aiff" => Aiff,
            ".ac3" => Ac3,
            ".adx" => Adx,
            ".wma" => Wma,
            _ => throw new EnumerationParseException(nameof(AudioFormat), filename)
        };
    }
}