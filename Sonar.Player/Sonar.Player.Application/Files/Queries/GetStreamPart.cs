using MediatR;
using Sonar.Player.Application.Tools;

namespace Sonar.Player.Application.Files.Queries;

public static class GetStreamPart
{
    public record Query(string fileName) : IRequest<Response>;

    public record Response(FileStream StreamPart);

    public class QueryHandler : IRequestHandler<Query, Response>
    {
        private readonly ITrackPathBuilder _builder;

        public QueryHandler(ITrackPathBuilder builder)
        {
            _builder = builder;
        }
        
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var filePath = _builder.GetTrackStreamPartPath(request.fileName);
            return new Response(File.OpenRead(filePath));
        }
    }
}