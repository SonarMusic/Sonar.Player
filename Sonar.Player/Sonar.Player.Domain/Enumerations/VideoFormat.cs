using Sonar.Player.Domain.Tools.Exceptions;

namespace Sonar.Player.Domain.Enumerations;

public class VideoFormat : MediaFormat
{
    protected VideoFormat()
    { }
    
    protected VideoFormat(string name, string format)
        : base(name, format) { }
    
    public static VideoFormat Mp4 => new VideoFormat("Mp4", "mp4");
    public static VideoFormat Mov => new VideoFormat("Mov", "mov");
    public static VideoFormat Wmv => new VideoFormat("Wmv", "wmv");

    public static VideoFormat FromFileName(string filename)
    {
        ArgumentNullException.ThrowIfNull(filename);
        return Path.GetExtension(filename) switch
        {
            ".mp4" => Mp4,
            ".mov" => Mov,
            ".wmv" => Wmv,
            _ => throw new EnumerationParseException(nameof(VideoFormat), filename)
        };
    }
}