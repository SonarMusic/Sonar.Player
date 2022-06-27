using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonar.Player.Domain.Entities;

namespace Sonar.Player.Data.Configurations;

public class QueueConfiguration : IEntityTypeConfiguration<TracksQueue>
{
    public void Configure(EntityTypeBuilder<TracksQueue> builder)
    {
        builder.Property<int>("id").ValueGeneratedOnAdd();
        builder.HasKey("id");
        builder.Property(q => q.Tracks)
            .HasField("_tracks")
            .HasConversion(
                p => string.Join(" ", p),
                p => p.Trim().Split().SkipWhile(s => s == "").Select(Guid.Parse).ToList());
    }
}