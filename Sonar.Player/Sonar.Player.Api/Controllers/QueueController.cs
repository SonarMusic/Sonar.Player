using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sonar.Player.Application.Queue.Commands;
using Sonar.Player.Application.Queue.Queries;
using Sonar.Player.Application.Services;

namespace Sonar.Player.Api.Controllers;

/// <summary>
/// API Controller to manage queues
/// </summary>
[ApiController]
[Route("[controller]")]
public class QueueController : Controller
{
    private readonly IMediator _mediator;
    private readonly IUserService _userService;

    /// <summary>
    /// Constructor lol
    /// </summary>
    /// <param name="mediator">Standard MediatR interface to interact with application layer</param>
    /// <param name="userService">Interface to get authorized users</param>
    public QueueController(IMediator mediator, IUserService userService)
    {
        _mediator = mediator;
        _userService = userService;
    }

    /// <summary>
    /// Get required queue
    /// </summary>
    /// <param name="token">Token to specify and verify user</param>
    /// <param name="cancellationToken">CancellationToken to observe while waiting for the task to complete</param>
    /// <returns>Queue filled with tracks</returns>
    [HttpGet]
    public async Task<ActionResult<GetQueue.Response>> GetQueueAsync(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return Ok(await _mediator.Send(new GetQueue.Query(user), cancellationToken));
    }
    
    /// <summary>
    /// Get next track in required queue
    /// </summary>
    /// <param name="token">Token to specify and verify user</param>
    /// <param name="cancellationToken">CancellationToken to observe while waiting for the task to complete</param>
    /// <returns>Track id and its name</returns>
    [HttpGet("next")]
    public async Task<ActionResult<GetNextTrack.Response>> GetNextTrackAsync(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return Ok(await _mediator.Send(new GetNextTrack.Query(user), cancellationToken));
    }
    
    /// <summary>
    /// Get previous track in required queue
    /// </summary>
    /// <param name="token">Token to specify and verify user</param>
    /// <param name="cancellationToken">CancellationToken to observe while waiting for the task to complete</param>
    /// <returns>Track id and its name</returns>
    [HttpGet("previous")]
    public async Task<ActionResult<GetPreviousTrack.Response>> GetPreviousTrackAsync(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return Ok(await _mediator.Send(new GetPreviousTrack.Query(user), cancellationToken));
    }
    
    /// <summary>
    /// Get current track in required queue
    /// </summary>
    /// <param name="token">Token to specify and verify user</param>
    /// <param name="cancellationToken">CancellationToken to observe while waiting for the task to complete</param>
    /// <returns>Track id and its name</returns>
    [HttpGet("current")]
    public async Task<ActionResult<GetCurrentTrack.Response>> GetCurrentTrackAsync(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return Ok(await _mediator.Send(new GetCurrentTrack.Query(user), cancellationToken));
    }

    /// <summary>
    /// Processes playlist to create new queue
    /// </summary>
    /// <param name="token">Token to specify and verify user</param>
    /// <param name="playlistId">Identification descriptor for existing playlist</param>
    /// <param name="cancellationToken">CancellationToken to observe while waiting for the task to complete</param>
    /// <returns>Nothing</returns>
    [HttpPatch("playlist")]
    public async Task<IActionResult> AddPlaylistToQueueAsync(
        [FromHeader(Name = "Token")] string token,
        [FromQuery] Guid playlistId, CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return Ok(await _mediator.Send(new AddPlaylistToQueue.Command(user, playlistId), cancellationToken));
    }
    
    /// <summary>
    /// Add track to required queue
    /// </summary>
    /// <param name="token">Token to specify and verify user</param>
    /// <param name="trackId">Identification descriptor for added track</param>
    /// <param name="cancellationToken">CancellationToken to observe while waiting for the task to complete</param>
    /// <returns>Nothing</returns>
    [HttpPatch("track")]
    public async Task<IActionResult> AddTrackToQueueAsync(
        [FromHeader(Name = "Token")] string token,
        [FromQuery] Guid trackId, CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return Ok(await _mediator.Send(new AddTrackToQueue.Command(user, trackId), cancellationToken));
    }

    /// <summary>
    /// Clear queue
    /// </summary>
    /// <param name="token">Token to specify and verify user</param>
    /// <param name="cancellationToken">CancellationToken to observe while waiting for the task to complete</param>
    /// <returns>Nothing</returns>
    [HttpDelete]
    public async Task<IActionResult> PurgeQueueAsync(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return Ok(await _mediator.Send(new PurgeQueue.Command(user), cancellationToken));
    }

    /// <summary>
    /// Randomize order of playing tracks
    /// </summary>
    /// <param name="token">Token to specify and verify user</param>
    /// <param name="cancellationToken">CancellationToken to observe while waiting for the task to complete</param>
    /// <returns>Nothing</returns>
    [HttpPatch("shuffle")]
    public async Task<IActionResult> ShuffleQueueAsync(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        return Ok(await _mediator.Send(new ShuffleQueue.Command(user), cancellationToken));
    }
}