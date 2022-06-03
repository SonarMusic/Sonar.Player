using Sonar.Player.Application.Services;
using Sonar.Player.Domain.Models;

namespace Sonar.Player.Fakes.Services;

public class FakeUserService : IUserService
{
    public Task<User> GetUserAsync(string token)
    {
        return Task.FromResult(new User(Guid.NewGuid(), token));
    }
}