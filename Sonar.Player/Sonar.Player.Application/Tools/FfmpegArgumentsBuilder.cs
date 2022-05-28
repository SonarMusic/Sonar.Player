using System.Text;

namespace Sonar.Player.Application.Tools;

public class FfmpegArgumentsBuilder : ISourceSelectionStage, IBitrateSelectionStage, ISegmentSelectionStage, IOutputSelectionStage, IBuildStage
{
    private readonly StringBuilder _builder;

    private FfmpegArgumentsBuilder()
    {
        _builder = new StringBuilder();
    }

    public static ISourceSelectionStage CreateBuilder()
    {
        return new FfmpegArgumentsBuilder();
    }

    public IBitrateSelectionStage GetSource(string sourcePath)
    {
        _builder.Append($"-i {sourcePath}");
        return this;
    }

    public ISegmentSelectionStage SetBitrate(int bitrate)
    {
        _builder.Append($" -bitrate {bitrate}");
        return this;
    }

    public IOutputSelectionStage SetSegmentFilename(string segmentName)
    {
        _builder.Append($" -f hls -hls_time 3 -hls_playlist_type vod -hls_flags independent_segments -hls_segment_type mpegts -hls_segment_filename {segmentName}");
        return this;
    }

    public IBuildStage WriteTo(string filename)
    {
        _builder.Append($" {filename}");
        return this;
    }

    public string Build()
    {
        return _builder.ToString();
    }
}

public interface ISourceSelectionStage
{
    IBitrateSelectionStage GetSource(string sourcePath);
}

public interface IBitrateSelectionStage
{
    ISegmentSelectionStage SetBitrate(int bitrate);
}

public interface ISegmentSelectionStage
{
    IOutputSelectionStage SetSegmentFilename(string segmentName);
}

public interface IOutputSelectionStage
{
    IBuildStage WriteTo(string filename);
}

public interface IBuildStage
{
    string Build();
}