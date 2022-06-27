using MediatR;
using Sonar.Player.Application.Tools;
using Sonar.Player.Application.Tools.Exceptions;
using Sonar.Player.Data;
using Sonar.Player.Domain.Entities;
using Sonar.Player.Domain.Models;
using Sonar.UserTracksManagement.ApiClient;

namespace Sonar.Player.Application.Queue.Queries;

public static class GetQueue
{
    public record Query(User User) : IRequest<Response>;

    public record Response(List<Response.Track> Tracks)
    {
        public record Track(Guid Id, string Name);
    };

    public class QueryHandler : IRequestHandler<Query, Response>
    {
        private readonly PlayerDbContext _dbContext;
        private readonly IUserTracksApiClient _tracksApiClient;

        public QueryHandler(PlayerDbContext dbContext, IUserTracksApiClient tracksApiClient)
        {
            _dbContext = dbContext;
            _tracksApiClient = tracksApiClient;
        }
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var context = await _dbContext.GetOrCreateContext(request.User);
            var newList = context.Queue.Tracks
                .Skip(context.Queue.CurrentNumber)
                .Select(async id => await _tracksApiClient.TracksGETAsync(request.User.Token, id, cancellationToken));

            await Task.WhenAll(newList);
            var tracks = newList.Select(t => new Response.Track(t.Result.Id, t.Result.Name)).ToList();

                
            return new Response(tracks);
        }
    }
}