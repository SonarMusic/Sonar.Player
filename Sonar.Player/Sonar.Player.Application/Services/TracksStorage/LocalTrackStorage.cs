using Sonar.Player.Application.Tools;
using Sonar.Player.Application.Tools.Exceptions;
using Sonar.Player.Domain.Entities;
using Sonar.Player.Domain.Enumerations;

namespace Sonar.Player.Application.Services.TracksStorage;

public class LocalTrackStorage : ITrackStorage
{
    private readonly ITrackPathBuilder _pathBuilder;

    public LocalTrackStorage(ITrackPathBuilder pathBuilder)
    {
        _pathBuilder = pathBuilder;
    }

    public async Task<Track> SaveTrack(Guid id, MediaFormat format, Stream content)
    {
        if (format is not AudioFormat audioFormat)
            throw new TrackFormatException($"Wrong track format - {format.Value}");

        var trackDirectory = _pathBuilder.GetTrackFolderPath(id);

        if (!Directory.Exists(trackDirectory))
            Directory.CreateDirectory(trackDirectory);

        var fileName = $"track.{audioFormat.Value}";
        var filePath = Path.Combine(trackDirectory, fileName);
        await using var fileStream = File.Create(filePath);
        await content.CopyToAsync(fileStream);

        return new Track(id, audioFormat, fileName);
    }

    public Task DeleteTrack(Guid id)
    {
        var trackDirectory = _pathBuilder.GetTrackFolderPath(id);
        if (Directory.Exists(trackDirectory))
            Directory.Delete(trackDirectory, true);

        return Task.CompletedTask;
    }
}