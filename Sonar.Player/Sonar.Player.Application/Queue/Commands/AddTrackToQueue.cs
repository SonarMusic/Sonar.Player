using MediatR;
using Sonar.Player.Application.Tools;
using Sonar.Player.Application.Tools.Exceptions;
using Sonar.Player.Data;
using Sonar.Player.Domain.Entities;
using Sonar.Player.Domain.Models;

namespace Sonar.Player.Application.Queue.Commands;

public static class AddTrackToQueue
{
    public record Command(User User, Guid TrackId) : IRequest<Response>;

    public record Response();

    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly PlayerDbContext _dbContext;

        public CommandHandler(PlayerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var track = _dbContext.Tracks.FirstOrDefault(x => x.Id == request.TrackId);
            var context = _dbContext.Contexts.GetOrCreateContext(request.User);

            if (track is null)
                throw new TrackNotFoundException($"Track {request.TrackId} is not found in database");
            
            context.Queue.Enqueue(track);
            _dbContext.Contexts.Update(context);
            return new Response();
        }
    }
}