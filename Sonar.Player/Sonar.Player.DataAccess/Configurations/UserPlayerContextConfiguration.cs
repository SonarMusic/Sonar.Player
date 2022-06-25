using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonar.Player.Domain.Entities;

namespace Sonar.Player.Data.Configurations;

public class UserPlayerContextConfiguration : IEntityTypeConfiguration<UserPlayerContext>
{
    public void Configure(EntityTypeBuilder<UserPlayerContext> builder)
    {
        builder.HasKey(x => x.User.Id);
    }
}