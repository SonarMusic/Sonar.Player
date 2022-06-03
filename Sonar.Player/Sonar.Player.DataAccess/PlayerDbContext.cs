using Microsoft.EntityFrameworkCore;
using Sonar.Player.Domain.Entities;

namespace Sonar.Player.Data;

public class PlayerDbContext : DbContext
{
    public DbSet<Track> Tracks { get; set; }

    public PlayerDbContext(DbContextOptions<PlayerDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}