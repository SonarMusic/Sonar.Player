using MediatR;
using Sonar.Player.Application.Tools;
using Sonar.Player.Application.Tools.Exceptions;
using Sonar.Player.Data;
using Sonar.Player.Domain.Entities;
using Sonar.Player.Domain.Models;

namespace Sonar.Player.Application.Queue.Commands;

public static class PurgeQueue
{
    public record Command(User User) : IRequest<Unit>;

    public class CommandHandler : IRequestHandler<Command, Unit>
    {
        private readonly PlayerDbContext _dbContext;

        public CommandHandler(PlayerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var context = await _dbContext.Contexts.GetOrCreateContext(request.User);
            context.Queue.Purge();
            _dbContext.Contexts.Update(context);
            return Unit.Value;
        }
    }
}