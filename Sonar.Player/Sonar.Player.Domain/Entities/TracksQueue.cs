namespace Sonar.Player.Domain.Entities;

public class TracksQueue
{
    private List<Track> _tracks;

    public TracksQueue()
    {
        _tracks = new List<Track>();
    }
    
    private TracksQueue(ICollection<Track> tracks)
    {
        _tracks = tracks.ToList();
    }

    public Track Next()
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