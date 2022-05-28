using System.Diagnostics;
using Sonar.Player.Application.Tools;
using Sonar.Player.Domain.Entities;
using Sonar.Player.Domain.Enumerations;

namespace Sonar.Player.Application.Services.TracksStorage;

public class HlsTrackProcessor : ITrackStorage
{
    private readonly ITrackStorage _decorated;
    private readonly ITrackPathBuilder _pathBuilder;
    
    public HlsTrackProcessor(ITrackStorage decorated, ITrackPathBuilder pathBuilder)
    {
        _decorated = decorated;
        _pathBuilder = pathBuilder;
    }

    public async Task<Track> SaveTrack(Guid id, AudioFormat format, Stream content)
    {
        var track = await _decorated.SaveTrack(id, format, content);

        var trackPath = Path.Combine(_pathBuilder.GetTrackFolderPath(track.Id), track.FileName);
        var streamFolderPath = _pathBuilder.GetTrackStreamFolderPath(track.Id);

        if (!Directory.Exists(streamFolderPath))
            Directory.CreateDirectory(streamFolderPath);

        var segmentFilename = $"{track.Id}-stream-%02d.ts";
        var segmentPath = Path.Combine(streamFolderPath, segmentFilename);
        var infoFilePath = Path.Combine(streamFolderPath, "streamInfo.m3u8");

        var arguments = new FfmpegArgumentsBuilder()
            .Source(trackPath)
            .Bitrate(4800)
            .SegmentFilename(segmentPath)
            .OutputFile(infoFilePath);

        Process process = new()
        {
            StartInfo = new()
            {
                FileName = "ffmpeg.exe",
                Arguments = arguments,
                UseShellExecute = false,
            },
            EnableRaisingEvents = true,
        };

        process.Start();
        await process.WaitForExitAsync();

        return track;
    }
}