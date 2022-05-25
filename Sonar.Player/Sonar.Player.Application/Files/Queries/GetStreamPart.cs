using MediatR;

namespace Sonar.Player.Application.Files.Queries;

public static class GetStreamPart
{
    public record Query() : IRequest<Response>;

    public record Response(FileStream StreamPart);

    public class QueryHandler : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}