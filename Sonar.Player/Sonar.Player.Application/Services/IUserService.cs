using Sonar.Player.Domain.Models;

namespace Sonar.Player.Application.Services;

public interface IUserService
{
    Task<User> GetUserAsync(string token, CancellationToken cancellationToken);
}