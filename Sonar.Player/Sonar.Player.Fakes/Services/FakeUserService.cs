using Sonar.Player.Application.Services;
using Sonar.Player.Domain.Models;

namespace Sonar.Player.Fakes.Services;

public class FakeUserService : IUserService
{
    public User GetUser(string token)
    {
        return new User(Guid.NewGuid(), token);
    }
}