using MediatR;

namespace Sonar.Player.Application.PlayerContext.Queries;

public static class SampleQuery
{
    public record Query() : IRequest<Response>;

    public record Response();

    public class QueryHandler : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}