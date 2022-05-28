using MediatR;
using Sonar.Player.Application.Services.TracksStorage;
using Sonar.Player.Application.Tools.Exceptions;
using Sonar.Player.Domain.Models;
using Sonar.UserTracksManagement.ApiClient;

namespace Sonar.Player.Application.Files.Commands;

public static class DeleteTrack
{
    public record Command(string Token, Guid TrackId) : IRequest<Unit>;

    public class CommandHandler : IRequestHandler<Command, Unit>
    {
        private readonly IUserTracksApiClient _userTracksApiClient;
        private readonly ITrackStorage _trackStorage;

        public CommandHandler(IUserTracksApiClient userTracksApiClient, ITrackStorage trackStorage)
        {
            _userTracksApiClient = userTracksApiClient;
            _trackStorage = trackStorage;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            bool isEnoughAccess = await _userTracksApiClient
                .IsEnoughAccessAsync(request.Token, request.TrackId, cancellationToken);

            if (!isEnoughAccess)
            {
                throw new NotEnoughAccessException($"Not enough access to track {request.TrackId}");
            }

            //TODO: call _userTracksApiClient.DeleteAsync
            await _trackStorage.DeleteTrack(request.TrackId);
            return Unit.Value;
        }
    }
}