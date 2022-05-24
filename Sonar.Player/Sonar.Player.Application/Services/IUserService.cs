using Sonar.Player.Domain.Models;

namespace Sonar.Player.Application.Services;

public interface IUserService
{
    User GetUser(string token);
}