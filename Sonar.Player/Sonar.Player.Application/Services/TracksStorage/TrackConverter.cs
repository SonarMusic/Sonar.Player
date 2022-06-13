using System.Diagnostics;
using Sonar.Player.Domain.Entities;
using Sonar.Player.Domain.Enumerations;

namespace Sonar.Player.Application.Services.TracksStorage;

public class TrackConverter : ITrackStorage
{
    private readonly ITrackStorage _decorated;

    public TrackConverter(ITrackStorage decorated)
    {
        _decorated = decorated;
    }

    public async Task<Track> SaveTrack(Guid id, MediaFormat format, Stream content)
    {
        if (format is AudioFormat)
            return await _decorated.SaveTrack(id, format, content);

        var tempDirectory = "temp";
        Directory.CreateDirectory(tempDirectory);

        var sourceFileName = $"{Guid.NewGuid()}.{format.Value}";
        var sourceFilePath = Path.Combine(tempDirectory, sourceFileName);
        await using (var sourceFileStream = File.OpenWrite(sourceFilePath))
        {
            await content.CopyToAsync(sourceFileStream);
        }

        var convertedFileName = $"{Guid.NewGuid()}.mp3";
        var convertedFilePath = Path.Combine(tempDirectory, convertedFileName);
        
        Process process = new()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-i {sourceFilePath} {convertedFilePath}",
                UseShellExecute = false,
            },
            EnableRaisingEvents = true,
        };

        process.Start();
        await process.WaitForExitAsync();
        
        File.Delete(sourceFilePath);
        
        Track track;
        await using (var convertedFileStream = File.OpenRead(convertedFilePath))
        {
            track = await _decorated.SaveTrack(id, AudioFormat.Mp3, convertedFileStream);
        }

        File.Delete(convertedFilePath);
        return track;
    }

    public async Task DeleteTrack(Guid id)
    {
        await _decorated.DeleteTrack(id);
    }
}