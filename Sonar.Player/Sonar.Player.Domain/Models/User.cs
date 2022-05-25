namespace Sonar.Player.Domain.Models;

public class User
{
    public Guid Id { get; }
    public string Token { get; }

    public User(Guid id, string token)
    {
        Id = id;
        Token = token;
    }
}