using MediatR;
using Sonar.Player.Application.Tools;
using Sonar.Player.Application.Tools.Exceptions;
using Sonar.Player.Data;
using Sonar.Player.Domain.Entities;
using Sonar.Player.Domain.Models;

namespace Sonar.Player.Application.Queue.Queries;

public static class GetQueue
{
    public record Query(User User) : IRequest<Response>;

    public record Response(List<Guid> TracksId);

    public class QueryHandler : IRequestHandler<Query, Response>
    {
        private readonly PlayerDbContext _dbContext;

        public QueryHandler(PlayerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var context = await _dbContext.Contexts.GetOrCreateContext(request.User);
            var current = context.Queue.CurrentNumber;
            var currentId = context.Queue.Tracks.ElementAt(current).Id;
            var newList = context.Queue.Tracks
                .SkipWhile(x => x.Id != currentId)
                .Select(x => x.Id)
                .ToList();
            
            return new Response(newList);
        }
    }
}