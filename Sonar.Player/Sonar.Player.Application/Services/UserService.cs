using Sonar.Player.Application.Tools.Exceptions;
using Sonar.Player.Domain.Models;
using Sonar.UserProfile.ApiClient;

namespace Sonar.Player.Application.Services;

public class UserService : IUserService
{
    private readonly IUserApiClient _apiClient;

    public UserService(IUserApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<User> GetUserAsync(string token)
    {
        try
        {
            var userDto = await _apiClient.GetAsync(token);
            return new User(userDto.Id, token);
        }
        catch (ApiException e)
        {
            throw new ExternalApiException(e.Response, e.StatusCode);
        }
    }
}