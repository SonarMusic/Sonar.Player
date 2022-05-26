namespace Sonar.Player.Application.Tools;

public class TrackPathBuilder : ITrackPathBuilder
{
    public string GetTrackFolderPath(Guid trackId)
    {
        //TODO: move base directory configuration to appsettings
        return Path.Combine(Directory.GetCurrentDirectory(), "tracks", trackId.ToString());
    }

    public string GetTrackStreamInfoPath(Guid trackId)
    {
        return Path.Combine(GetTrackFolderPath(trackId), "stream", "streamInfo.m3u8");
    }

    public string GetTackStreamPartPath(Guid trackId, string partName)
    {
        return Path.Combine(GetTrackFolderPath(trackId), "stream", partName);
    }
}