namespace Sonar.Player.Application.Tools;

public interface ITrackPathBuilder
{
    string GetTrackFolderPath(Guid trackId);
    string GetTrackStreamInfoPath(Guid trackId);
    string GetTackStreamPartPath(Guid trackId, string partName);
}