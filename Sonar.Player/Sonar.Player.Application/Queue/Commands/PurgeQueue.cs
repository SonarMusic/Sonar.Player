using MediatR;
using Sonar.Player.Application.Tools.Exceptions;
using Sonar.Player.Data;
using Sonar.Player.Domain.Entities;
using Sonar.Player.Domain.Models;

namespace Sonar.Player.Application.Queue.Commands;

public static class PurgeQueue
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
            var context = _dbContext.Contexts.FirstOrDefault(x => x.User.Id == request.User.Id);
            if (context is null)
                throw new NotFoundException("Queue of this user is not found in database");

            var queue = context.Queue;
            queue.Purge();
            _dbContext.Remove(context);
            await _dbContext.Contexts.AddAsync(new UserPlayerContext(request.User, queue), cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new Response(queue);
        }
    }
}