using MediatR;
using Sonar.Player.Application.Tools;
using Sonar.Player.Application.Tools.Exceptions;
using Sonar.UserTracksManagement.ApiClient;

namespace Sonar.Player.Application.Files.Queries;

public static class GetTrackStreamInfo
{
    public record Query(string Token, Guid TrackId) : IRequest<Response>;

    public record Response(FileStream TrackInfoStream);

    public class QueryHandler : IRequestHandler<Query, Response>
    {
        private readonly ITrackPathBuilder _builder;
        private readonly IUserTracksApiClient _apiClient;

        public QueryHandler(ITrackPathBuilder builder, IUserTracksApiClient apiClient)
        {
            _builder = builder;
            _apiClient = apiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            bool isEnoughAccess = await _apiClient.IsEnoughAccessAsync(
                request.Token,
                request.TrackId,
                cancellationToken);

            if (!isEnoughAccess)
            {
                throw new NotEnoughAccessException($"Not enough access to track {request.TrackId}");
            }

            var path = _builder.GetTrackStreamInfoPath(request.TrackId);
            return new Response(File.OpenRead(path));
        }
    }
}
