using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonar.Player.Domain.Entities;
using Sonar.Player.Domain.Enumerations;

namespace Sonar.Player.Data.Configurations;

public class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.OwnsOne<MediaFormat>(f => f.Format);
    }
}