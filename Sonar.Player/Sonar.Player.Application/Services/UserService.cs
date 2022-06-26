using Sonar.Player.Application.Tools.Exceptions;
using Sonar.Player.Domain.Models;
using Sonar.UserProfile.ApiClient;
using Sonar.UserProfile.ApiClient.Interfaces;
using Sonar.UserTracksManagement.ApiClient;

namespace Sonar.Player.Application.Services;

public class UserService : IUserService
{
    private readonly IUserApiClient _apiClient;

    public UserService(IUserApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<User> GetUserAsync(string token, CancellationToken cancellationToken)
    {
        try
        {
            var userDto = await _apiClient.GetAsync(token, cancellationToken);
            return new User(userDto.Id, token);
        }
        catch (ApiException e)
        {
            throw new ExternalApiException(e.Response, e.StatusCode);
        }
    }
}