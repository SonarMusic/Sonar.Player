using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sonar.Player.Api.Controllers.Dto;
using Sonar.Player.Application.Import.Commands;
using Sonar.Player.Application.Services;

namespace Sonar.Player.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ImportController : Controller
{
    private readonly IMediator _mediator;
    private readonly IUserService _userService;

    public ImportController(IMediator mediator, IUserService userService)
    {
        _mediator = mediator;
        _userService = userService;
    }

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