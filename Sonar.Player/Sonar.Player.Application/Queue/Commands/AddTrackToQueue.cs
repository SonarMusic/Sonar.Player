using MediatR;
using Sonar.Player.Application.Tools.Exceptions;
using Sonar.Player.Data;
using Sonar.Player.Domain.Entities;
using Sonar.Player.Domain.Models;

namespace Sonar.Player.Application.Queue.Commands;

public static class AddTrackToQueue
{
    public record Command(User User, Guid TrackId) : IRequest<Response>;

    public record Response(Guid TrackId);

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
            var track = _dbContext.Tracks.FirstOrDefault(x => x.Id == request.TrackId);
            TracksQueue queue;
            if (context is null)
            {
                queue = new TracksQueue();
            }
            else
            {
                queue = context.Queue;
                _dbContext.Contexts.Remove(context);
            }

            if (track is null)
                throw new NotFoundException("Track is not found in database");
            
            queue.Enqueue(track);
            await _dbContext.Contexts.AddAsync(new UserPlayerContext(request.User, queue), cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new Response(request.TrackId);
        }
    }
}