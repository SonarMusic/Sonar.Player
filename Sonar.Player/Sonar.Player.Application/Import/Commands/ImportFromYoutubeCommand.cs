using MediatR;
using Sonar.Player.Application.Services.TracksStorage;
using Sonar.Player.Domain.Models;
using Sonar.UserTracksManagement.ApiClient;
using VideoLibrary;
using VideoFormat = Sonar.Player.Domain.Enumerations.VideoFormat;

namespace Sonar.Player.Application.Import.Commands;

public static class ImportFromYoutubeCommand
{
    public record Command(User User, string Url, string? Name) : IRequest<Response>;

    public record Response(Guid TrackId);

    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly IUserTracksApiClient _userTracksApiClient;
        private readonly ITrackStorage _storage;

        public CommandHandler(IUserTracksApiClient userTracksApiClient, ITrackStorage storage)
        {
            _userTracksApiClient = userTracksApiClient;
            _storage = storage;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var youtube = YouTube.Default;
            var video = await youtube.GetVideoAsync(request.Url);

            var name = request.Name ?? video.Title;
            var trackId = await _userTracksApiClient.TracksPOSTAsync(request.User.Token, name, cancellationToken);
            
            await using var videoStream = await video.StreamAsync();
            await _storage.SaveTrack(trackId, VideoFormat.Mp4, videoStream);

            return new Response(trackId);
        }
    }
}