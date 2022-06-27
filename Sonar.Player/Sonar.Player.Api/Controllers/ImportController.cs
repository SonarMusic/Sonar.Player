using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sonar.Player.Api.Controllers.Dto;
using Sonar.Player.Application.Import.Commands;
using Sonar.Player.Application.Services;

namespace Sonar.Player.Api.Controllers;

/// <summary>
/// API Controller to import tracks from external services
/// </summary>
[ApiController]
[Route("[controller]")]
public class ImportController : Controller
{
    private readonly IMediator _mediator;
    private readonly IUserService _userService;

    /// <summary>
    /// Constructor lol
    /// </summary>
    /// <param name="mediator">Standard MediatR interface to interact with application layer</param>
    /// <param name="userService">Interface to get authorized users</param>
    public ImportController(IMediator mediator, IUserService userService)
    {
        _mediator = mediator;
        _userService = userService;
    }

    /// <summary>
    /// Add track to service in user's profile
    /// </summary>
    /// <param name="token">Token to specify and verify user</param>
    /// <param name="trackInfo">Link to track and its name</param>
    /// <param name="cancellationToken">CancellationToken to observe while waiting for the task to complete</param>
    /// <returns>Identification descriptor for added track</returns>
    [HttpPost("youtube")]
    public async Task<ActionResult<ImportFromYoutubeCommand.Response>> ImportFromYoutube(
        [FromHeader(Name = "Token")] string token,
        [FromBody] YoutubeTrackInfoDto trackInfo,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return await _mediator.Send(
            new ImportFromYoutubeCommand.Command(user, trackInfo.Url, trackInfo.Name),
            cancellationToken);
    }
}