using Sonar.Player.Domain.Tools.Exceptions;

namespace Sonar.Player.Domain.Entities;

public class TracksQueue
{
    private readonly List<Track> _tracks = new List<Track>();

    public Track? Current = null;
    public IReadOnlyCollection<Track> Tracks => _tracks.AsReadOnly();

    public Track Next()
    {
        if (Current is null)
            throw new SonarPlayerException("Cannot get the next track: queue is empty");
        
        var next = Tracks.SkipWhile(x => x.Id != Current.Id).Skip(1).DefaultIfEmpty(null).First();
        Current = next ?? throw new SonarPlayerException("Cannot get the next track: this is the last track in queue");
        return Current;
    }

    public void Enqueue(Track track)
    {
        _tracks.Add(track);
    }

    public Track Previous()
    {
        if (Current is null)
            throw new SonarPlayerException("Cannot get the last track: queue is empty");

        var prev = Tracks.TakeWhile(x => x.Id != Current.Id).DefaultIfEmpty(null).Last();
        Current = prev ?? throw new SonarPlayerException("Cannot get the last track: this is the last track in queue");
        return Current;
    }

    public void Shuffle()
    {
        var rand = new Random();
        var newTracks = _tracks.OrderBy(x => rand.Next()).ToList();
        for (var i = 0; i < _tracks.Count; i++)
            _tracks[i] = newTracks[i];
    }

    public void Purge()
    {
        _tracks.Clear();
    }
}