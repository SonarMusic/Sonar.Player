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

    public record Response(TracksQueue Queue);

    public class QueryHandler : IRequestHandler<Query, Response>
    {
        private readonly PlayerDbContext _dbContext;

        public QueryHandler(PlayerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var context = _dbContext.Contexts.GetOrCreateContext(request.User);
            return new Response(context.Queue);
        }
    }
}