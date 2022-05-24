namespace Sonar.Player.Domain.Entities;

public class TracksQueue
{
    private readonly List<Track> _tracks = new List<Track>();

    public IReadOnlyCollection<Track> Tracks => _tracks.AsReadOnly();

    public Track Next()
    {
        throw new NotImplementedException();
    }

    public void Enqueue(Track track)
    {
        throw new NotImplementedException();
    }

    public Track Previous()
    {
        throw new NotImplementedException();
    }

    public void Shuffle()
    {
        throw new NotImplementedException();
    }

    public void Purge()
    {
        throw new NotImplementedException();
    }
}