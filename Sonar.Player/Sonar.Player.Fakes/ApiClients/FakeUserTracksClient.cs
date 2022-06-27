using Sonar.UserTracksManagement.ApiClient;

namespace Sonar.Player.Fakes.ApiClients;

public class FakeUserTracksClient : IUserTracksApiClient
{
    private readonly List<TrackDto> _tracks = new List<TrackDto>();

    public Task<Guid> TracksPOSTAsync(string token, string name)
    {
        return TracksPOSTAsync(token, name, CancellationToken.None);
    }

    public Task<Guid> TracksPOSTAsync(string token, string name, CancellationToken cancellationToken)
    {
        var tdto = new TrackDto()
        {
            Name = name,
            Id = Guid.NewGuid()
        };
        _tracks.Add(tdto);
        return Task.FromResult(tdto.Id);
    }

    public Task<TrackDto> TracksGETAsync(string token, Guid? trackId)
    {
        return TracksGETAsync(token, trackId);
    }

    public Task<TrackDto> TracksGETAsync(string token, Guid? trackId, CancellationToken cancellationToken)
    {
        return Task.FromResult(_tracks.First(t => t.Id == trackId));
    }

    public Task TracksDELETEAsync(string token, Guid? trackId)
    {
        throw new NotImplementedException();
    }

    public Task TracksDELETEAsync(string token, Guid? trackId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<TrackDto>> All2Async(string token)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<TrackDto>> All2Async(string token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> PrivateAsync(string token, Guid? trackId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> PrivateAsync(string token, Guid? trackId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> PublicAsync(string token, Guid? trackId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> PublicAsync(string token, Guid? trackId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> OnlyFansAsync(string token, Guid? trackId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> OnlyFansAsync(string token, Guid? trackId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<TrackDto>> AllAsync(string token)
    {
        return AllAsync(token, CancellationToken.None);
    }

    public Task<ICollection<TrackDto>> AllAsync(string token, CancellationToken cancellationToken)
    {
        return Task.FromResult<ICollection<TrackDto>>(_tracks.ToList());
    }

    public Task<bool> IsEnoughAccessAsync(string token, Guid? trackId)
    {
        return IsEnoughAccessAsync(token, trackId, CancellationToken.None);
    }

    public Task<bool> IsEnoughAccessAsync(string token, Guid? trackId, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }

    public Task<ICollection<Playlist>> WithTag2Async(string token, string tag)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<Playlist>> WithTag2Async(string token, string tag, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}