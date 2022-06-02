using Sonar.Player.Domain.Tools.Exceptions;

namespace Sonar.Player.Domain.Entities;

public class TracksQueue
{
    private List<Track> _tracks = new List<Track>();

    public int CurrentNumber = 0;
    public IReadOnlyCollection<Track> Tracks => _tracks.AsReadOnly();

    public Track Next()
    {
        if (Tracks.Count == 0)
            throw new SonarPlayerException("Cannot get the next track: queue is empty");
        
        var next = Tracks.ElementAtOrDefault(CurrentNumber + 1);
        if (next is null)
            throw new SonarPlayerException("Cannot get the next track: this is the last track in queue");

        CurrentNumber++;
        return next;
    }

    public void Enqueue(Track track)
    {
        _tracks.Add(track);
        if (_tracks.Count == 1)
            CurrentNumber++;
    }

    public Track Previous()
    {
        if (Tracks.Count == 0)
            throw new SonarPlayerException("Cannot get the previous track: queue is empty");
        
        var prev = Tracks.ElementAtOrDefault(CurrentNumber - 1);
        if (prev is null)
            throw new SonarPlayerException("Cannot get the previous track: this is the first track in queue");

        CurrentNumber--;
        return prev;
    }

    public void Shuffle()
    {
        var rand = new Random();
        var newTracks = _tracks.OrderBy(x => rand.Next()).ToList();
        _tracks = newTracks;
    }

    public void Purge()
    {
        _tracks.Clear();
    }
}