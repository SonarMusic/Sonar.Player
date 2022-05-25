using MediatR;

namespace Sonar.Player.Application.Files.Queries;

public static class GetTrackStreamInfo
{
    public record Query() : IRequest<Response>;

    public record Response(FileStream TrackInfoStream);

    public class QueryHandler : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}