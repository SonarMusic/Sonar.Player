using Sonar.UserTracksManagement.ApiClient;

namespace Sonar.Player.Fakes.ApiClients;

public class FakeUserTracksClient : IUserTracksApiClient
{
    public Task<Guid> TracksPOSTAsync(string token, string name)
    {
        return Task.FromResult(Guid.NewGuid());
    }

    public Task<Guid> TracksPOSTAsync(string token, string name, CancellationToken cancellationToken)
    {
        return Task.FromResult(Guid.NewGuid());
    }

    public Task<TrackDto> TracksGETAsync(string token, Guid? trackId)
    {
        throw new NotImplementedException();
    }

    public Task<TrackDto> TracksGETAsync(string token, Guid? trackId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<TrackDto>> AllAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<TrackDto>> AllAsync(string token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsEnoughAccessAsync(string token, Guid? trackId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsEnoughAccessAsync(string token, Guid? trackId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}