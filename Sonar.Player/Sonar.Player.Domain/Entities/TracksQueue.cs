using Sonar.Player.Domain.Tools.Exceptions;

namespace Sonar.Player.Domain.Entities;

public class TracksQueue
{
    private List<Guid> _tracks = new List<Guid>();

    public int CurrentNumber { get; private set; } = -1;
    public IReadOnlyCollection<Guid> Tracks => _tracks.AsReadOnly();

    public Guid Previous()
    {
        if (Tracks.Count == 0)
            throw new SonarPlayerException("Cannot get the previous track: queue is empty");
        
        var prev = Tracks.ElementAtOrDefault(CurrentNumber - 1);
        if (prev == default)
            throw new SonarPlayerException("Cannot get the previous track: this is the first track in queue");

        CurrentNumber--;
        return prev;
    }

    public Guid Current()
    {
        if (Tracks.Count == 0)
            throw new SonarPlayerException("Cannot get the previous track: queue is empty");

        return _tracks.ElementAt(CurrentNumber);
    }
    
    public Guid Next()
    {
        if (Tracks.Count == 0)
            throw new SonarPlayerException("Cannot get the next track: queue is empty");
        
        var next = Tracks.ElementAtOrDefault(CurrentNumber+1);
        if (next == default)
            throw new SonarPlayerException("Cannot get the next track: this is the last track in queue");

        CurrentNumber++;
        return next;
    }

    public void Enqueue(Track track)
    {
        if (_tracks.Count != 0 && _tracks.Last() == track.Id)
            return;
        _tracks.Add(track.Id);
    }

    public void EnqueuePlaylistTracks(List<Track> tracksList)
    {
        _tracks.AddRange(tracksList.Select(t => t.Id));
        RemoveAdjacentDuplicates();
    }

    public void Shuffle()
    {
        var rand = new Random();
        var currentId = _tracks.ElementAtOrDefault(CurrentNumber);
        var newTracks = _tracks.Skip(CurrentNumber)
            .OrderBy(x => rand.Next()).ToList();
        _tracks = newTracks;
        CurrentNumber = 0;
        RemoveAdjacentDuplicates();
    }

    public void Purge()
    {
        _tracks.Clear();
        CurrentNumber = -1;
    }
    
    private void RemoveAdjacentDuplicates()
    {
        var newTracks = new List<Guid>();
        for (int i = 0; i < _tracks.Count; i++)
        {
            if (i == _tracks.Count-1 || _tracks[i] != _tracks[i + 1])
                newTracks.Add(_tracks[i]);
        }
        _tracks = newTracks;
        while (CurrentNumber >= _tracks.Count)
            CurrentNumber = _tracks.Count - 1;
    }
}