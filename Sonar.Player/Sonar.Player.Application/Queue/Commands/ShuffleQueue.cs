using System.Linq;
using MediatR;
using Sonar.Player.Application.Tools;
using Sonar.Player.Application.Tools.Exceptions;
using Sonar.Player.Data;
using Sonar.Player.Domain.Entities;
using Sonar.Player.Domain.Models;

namespace Sonar.Player.Application.Queue.Commands;

public static class ShuffleQueue
{
    public record Command(User User) : IRequest<Response>;

    public record Response(TracksQueue Queue);

    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly PlayerDbContext _dbContext;

        public CommandHandler(PlayerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var queue = _dbContext.Contexts.GetOrCreateContext(request.User);
            queue.Shuffle();
            _dbContext.Contexts.Update(new UserPlayerContext(request.User, queue));
            return new Response(queue);
        }
    }
}