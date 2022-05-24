namespace Sonar.Player.Domain.Models;

public class User
{
    public Guid Id { get; }

    public User(Guid id)
    {
        Id = id;
    }
}