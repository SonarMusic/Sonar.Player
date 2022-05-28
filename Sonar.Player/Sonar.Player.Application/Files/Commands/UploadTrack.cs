using MediatR;
using Sonar.Player.Application.Services.TracksStorage;
using Sonar.Player.Application.Tools.Exceptions;
using Sonar.Player.Data;
using Sonar.Player.Domain.Enumerations;
using Sonar.Player.Domain.Models;
using Sonar.UserTracksManagement.ApiClient;

namespace Sonar.Player.Application.Files.Commands;

public static class UploadTrack
{
    public record Command(User User, string Name, Command.TrackFile File) : IRequest<Response>
    {
        public record TrackFile(string Name, Stream Content);
    };

    public record Response(Guid trackId);

    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly IUserTracksApiClient _userTracksApiClient;
        private readonly ITrackStorage _trackStorage;
        private readonly PlayerDbContext _dbContext;

        public CommandHandler(IUserTracksApiClient userTracksApiClient, ITrackStorage trackStorage, PlayerDbContext dbContext)
        {
            _userTracksApiClient = userTracksApiClient;
            _trackStorage = trackStorage;
            _dbContext = dbContext;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            Guid trackId;
            try
            {
                trackId = await _userTracksApiClient.TracksPOSTAsync(request.User.Token, request.Name, cancellationToken);
            }
            catch (ApiException e)
            {
                throw new ExternalApiException(e.Response, e.StatusCode);
            }

            var track = await _trackStorage.SaveTrack(trackId, AudioFormat.FromFileName(request.File.Name), request.File.Content);
            await _dbContext.Tracks.AddAsync(track, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return new Response(trackId);
        }
    }
}