namespace Sonar.Player.Application.Tools;

public class TrackPathBuilder : ITrackPathBuilder
{
    public string GetTrackFolderPath(Guid trackId)
    {
        //TODO: move base directory configuration to appsettings
        return Path.Combine(Directory.GetCurrentDirectory(), "tracks", trackId.ToString());
    }

    public string GetTrackStreamFolderPath(Guid trackId)
    {
        return Path.Combine(GetTrackFolderPath(trackId), "stream");
    }

    public string GetTrackStreamInfoPath(Guid trackId)
    {
        return Path.Combine(GetTrackStreamFolderPath(trackId), "streamInfo.m3u8");
    }

    public string GetTackStreamPartPath(string partName)
    {
        var trackId = partName.Split("-").First();
        return Path.Combine(GetTrackFolderPath(Guid.Parse(trackId)), "stream", partName);
    }
}