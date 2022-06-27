using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sonar.Player.Application.Queue.Commands;
using Sonar.Player.Application.Queue.Queries;
using Sonar.Player.Application.Services;

namespace Sonar.Player.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class QueueController : Controller
{
    private readonly IMediator _mediator;
    private readonly IUserService _userService;

    public QueueController(IMediator mediator, IUserService userService)
    {
        _mediator = mediator;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<GetQueue.Response>> GetQueueAsync(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return Ok(await _mediator.Send(new GetQueue.Query(user), cancellationToken));
    }
    
    [HttpGet("next")]
    public async Task<ActionResult<GetNextTrack.Response>> GetNextTrackAsync(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return Ok(await _mediator.Send(new GetNextTrack.Query(user), cancellationToken));
    }
    
    [HttpGet("previous")]
    public async Task<ActionResult<GetPreviousTrack.Response>> GetPreviousTrackAsync(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return Ok(await _mediator.Send(new GetPreviousTrack.Query(user), cancellationToken));
    }
    
    [HttpGet("current")]
    public async Task<ActionResult<GetCurrentTrack.Response>> GetCurrentTrackAsync(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return Ok(await _mediator.Send(new GetCurrentTrack.Query(user), cancellationToken));
    }

    [HttpPatch("playlist")]
    public async Task<IActionResult> AddPlaylistToQueueAsync(
        [FromHeader(Name = "Token")] string token,
        [FromQuery] Guid playlistId, CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return Ok(await _mediator.Send(new AddPlaylistToQueue.Command(user, playlistId), cancellationToken));
    }
    
    [HttpPatch("track")]
    public async Task<IActionResult> AddTrackToQueueAsync(
        [FromHeader(Name = "Token")] string token,
        [FromQuery] Guid trackId, CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return Ok(await _mediator.Send(new AddTrackToQueue.Command(user, trackId), cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> PurgeQueueAsync(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return Ok(await _mediator.Send(new PurgeQueue.Command(user), cancellationToken));
    }

    [HttpPatch("shuffle")]
    public async Task<IActionResult> ShuffleQueueAsync(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return Ok(await _mediator.Send(new ShuffleQueue.Command(user), cancellationToken));
    }
}