using MediatR;
using Sonar.Player.Application.Tools;
using Sonar.Player.Application.Tools.Exceptions;
using Sonar.Player.Data;
using Sonar.Player.Domain.Entities;
using Sonar.Player.Domain.Models;
using Sonar.UserTracksManagement.ApiClient;
using Track = Sonar.Player.Domain.Entities.Track;

namespace Sonar.Player.Application.Queue.Commands;

public static class AddPlaylistToQueue
{
    public record Command(User User, Guid PlaylistId) : IRequest<Unit>;

    public class CommandHandler : IRequestHandler<Command, Unit>
    {
        private readonly IPlaylistApiClient _playlistApiClient;
        private readonly PlayerDbContext _dbContext;

        public CommandHandler(IPlaylistApiClient playlistApiClient, PlayerDbContext dbContext)
        {
            _playlistApiClient = playlistApiClient;
            _dbContext = dbContext;
        }
        
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            Playlist playlist;
            try
            {
                playlist = await _playlistApiClient.PlaylistGETAsync(request.User.Token, request.PlaylistId, cancellationToken);
            }
            catch (ApiException e)
            {
                throw new ExternalApiException(e.Response, e.StatusCode);
            }
            
            var context = await _dbContext.GetOrCreateContext(request.User);
            context.Queue.EnqueuePlaylistTracks(playlist.Tracks.Select(t => t.Track.Id).ToList());
            _dbContext.Update(context.Queue);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}