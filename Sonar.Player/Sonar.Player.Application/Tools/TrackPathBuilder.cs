using Microsoft.Extensions.Options;

namespace Sonar.Player.Application.Tools;

public class TrackPathBuilder : ITrackPathBuilder
{
    private readonly string _basePath;
    
    public TrackPathBuilder(IOptions<TrackPathBuilderConfiguration> options)
    {
        _basePath = options.Value.BasePath;
    }
    
    public string GetTrackFolderPath(Guid trackId)
    {
        return Path.Combine(_basePath, trackId.ToString());
    }

    public string GetTrackStreamFolderPath(Guid trackId)
    {
        return Path.Combine(GetTrackFolderPath(trackId), "stream");
    }

    public string GetTrackStreamInfoPath(Guid trackId)
    {
        return Path.Combine(GetTrackStreamFolderPath(trackId), "streamInfo.m3u8");
    }

    public string GetTrackStreamPartPath(string partName)
    {
        var trackId = partName.Split("_").First();
        return Path.Combine(GetTrackFolderPath(Guid.Parse(trackId)), "stream", partName);
    }
}