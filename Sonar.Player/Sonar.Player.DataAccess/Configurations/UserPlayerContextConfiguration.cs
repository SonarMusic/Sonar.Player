using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonar.Player.Domain.Entities;

namespace Sonar.Player.Data.Configurations;

public class UserPlayerContextConfiguration : IEntityTypeConfiguration<UserPlayerContext>
{
    public void Configure(EntityTypeBuilder<UserPlayerContext> builder)
    {
        builder.HasKey(upc => upc.UserId);
        builder.HasOne(upc => upc.Queue);
        builder.Navigation(upc => upc.Queue).AutoInclude();
    }
}