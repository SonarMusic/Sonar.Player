using System.Text;

namespace Sonar.Player.Application.Tools;

public class FfmpegArgumentsBuilder
{
    private readonly StringBuilder _builder;

    public FfmpegArgumentsBuilder()
    {
        _builder = new StringBuilder();
    }

    public FfmpegArgumentsBuilder Source(string sourcePath)
    {
        _builder.Append($"-i {sourcePath}");
        return this;
    }

    public FfmpegArgumentsBuilder Bitrate(int bitrate)
    {
        _builder.Append($" -bitrate {bitrate}");
        return this;
    }

    public FfmpegArgumentsBuilder SegmentFilename(string segmentName)
    {
        _builder.Append($" -f hls -hls_time 3 -hls_playlist_type vod -hls_flags independent_segments -hls_segment_type mpegts -hls_segment_filename {segmentName}");
        return this;
    }

    public string OutputFile(string filename)
    {
        _builder.Append($" {filename}");
        return _builder.ToString();
    }
}