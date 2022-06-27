using System.Linq;
using MediatR;
using Sonar.Player.Application.Services.TracksStorage;
using Sonar.Player.Application.Tools;
using Sonar.Player.Application.Tools.Exceptions;
using Sonar.Player.Data;
using Sonar.Player.Domain.Entities;
using Sonar.Player.Domain.Enumerations;
using Sonar.Player.Domain.Models;
using Sonar.UserTracksManagement.ApiClient;

namespace Sonar.Player.Application.Properties.Bitrate.Commands;

public static class SetBitrate
{
    public record Command(string Token, Guid TrackId, string Name, Command.TrackFile File, int Bitrate) : IRequest<Response>
    {
        public record TrackFile(string Name, Stream Content);
    };

    public record Response(Guid TrackId);

    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly PlayerDbContext _dbContext;
        private readonly IUserTracksApiClient _userTracksApiClient;
        private readonly ITrackStorage _trackStorage;

        public CommandHandler(PlayerDbContext dbContext, IUserTracksApiClient userTracksApiClient, ITrackStorage trackStorage)
        {
            _dbContext = dbContext;
            _userTracksApiClient = userTracksApiClient;
            _trackStorage = trackStorage;
        }
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            bool isEnoughAccess = await _userTracksApiClient.IsEnoughAccessAsync(
                request.Token,
                request.TrackId,
                cancellationToken);

            if (!isEnoughAccess)
            {
                throw new NotEnoughAccessException($"Not enough access to track {request.TrackId}");
            }

            await _trackStorage.DeleteTrack(request.TrackId);
            
            Settings.Bitrate = request.Bitrate;
            var track = await _trackStorage.SaveTrack(request.TrackId, AudioFormat.FromFileName(request.File.Name), request.File.Content);

            _dbContext.Tracks.Update(track);

            return new Response(request.TrackId);
        }
    }
}