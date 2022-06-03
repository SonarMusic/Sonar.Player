using Sonar.Player.Domain.Enumerations;

namespace Sonar.Player.Domain.Entities;

public class Track
{
    public Guid Id { get; private init; }
    public AudioFormat Format { get; private init; }
    public string FileName { get; private init; }
    
    public Track(Guid id, AudioFormat format, string fileName)
    {
        Id = id;
        Format = format;
        FileName = fileName;
    }

    private Track(string fileName)
    {
        FileName = fileName;
    }
}