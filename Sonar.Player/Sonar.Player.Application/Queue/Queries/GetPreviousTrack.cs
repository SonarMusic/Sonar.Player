using MediatR;
using Sonar.Player.Application.Tools;
using Sonar.Player.Data;
using Sonar.Player.Domain.Models;
using Sonar.UserTracksManagement.ApiClient;

namespace Sonar.Player.Application.Queue.Queries;

public static class GetPreviousTrack
{
    public record Query(User User) : IRequest<Response>;

    public record Response(Guid Id, string Name);

    public class CommandHandler : IRequestHandler<Query, Response>
    {
        private readonly PlayerDbContext _dbContext;
        private readonly IUserTracksApiClient _tracksApiClient;

        public CommandHandler(PlayerDbContext dbContext, IUserTracksApiClient tracksApiClient)
        {
            _dbContext = dbContext;
            _tracksApiClient = tracksApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var context = await _dbContext.GetOrCreateContext(request.User);
            var trackId = context.Queue.Previous();
            var trackInfo = await _tracksApiClient.TracksGETAsync(request.User.Token, trackId, cancellationToken);
            _dbContext.Update(context.Queue);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new Response(trackId, trackInfo.Name);
        }
    }
}