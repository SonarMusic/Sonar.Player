using MediatR;
using Sonar.Player.Application.Services.TracksStorage;
using Sonar.Player.Data;
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
        private readonly PlayerDbContext _dbContext;
        private readonly ITrackStorage _storage;

        public CommandHandler(IUserTracksApiClient userTracksApiClient, ITrackStorage storage, PlayerDbContext dbContext)
        {
            _userTracksApiClient = userTracksApiClient;
            _storage = storage;
            _dbContext = dbContext;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var youtube = YouTube.Default;
            var video = await youtube.GetVideoAsync(request.Url);

            var name = request.Name ?? video.Title;
            var trackId = await _userTracksApiClient.TracksPOSTAsync(request.User.Token, name, cancellationToken);
            
            await using var videoStream = await video.StreamAsync();
            var track = await _storage.SaveTrack(trackId, VideoFormat.Mp4, videoStream);
            await _dbContext.Tracks.AddAsync(track, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new Response(trackId);
        }
    }
}