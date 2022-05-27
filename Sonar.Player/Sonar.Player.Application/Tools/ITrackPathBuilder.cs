namespace Sonar.Player.Application.Tools;

public interface ITrackPathBuilder
{
    string GetTrackFolderPath(Guid trackId);
    string GetTrackStreamFolderPath(Guid trackId);
    string GetTrackStreamInfoPath(Guid trackId);
    string GetTrackStreamPartPath(string partName);
}