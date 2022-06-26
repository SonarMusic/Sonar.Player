using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonar.Player.Domain.Entities;

namespace Sonar.Player.Data.Configurations;

public class QueueConfiguration : IEntityTypeConfiguration<TracksQueue>
{
    public void Configure(EntityTypeBuilder<TracksQueue> builder)
    {
        builder.Property<int>("id");
        builder.HasKey("id");
        builder.HasMany(q => q.Tracks);
    }
}