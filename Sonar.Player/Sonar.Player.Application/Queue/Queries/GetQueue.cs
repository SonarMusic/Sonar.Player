using MediatR;

namespace Sonar.Player.Application.Queue.Queries;

public static class GetQueue
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