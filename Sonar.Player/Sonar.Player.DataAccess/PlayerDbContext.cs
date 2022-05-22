using Microsoft.EntityFrameworkCore;

namespace Sonar.Player.Data;

public class PlayerDbContext : DbContext
{
    public PlayerDbContext(DbContextOptions<PlayerDbContext> options)
        : base(options)
    {
    }
}