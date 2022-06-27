using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sonar.Player.Api.Controllers.Dto;
using Sonar.Player.Application.Files.Commands;
using Sonar.Player.Application.Files.Queries;
using Sonar.Player.Application.Services;

namespace Sonar.Player.Api.Controllers;

/// <summary>
/// API Controller to manage tracks
/// </summary>
[ApiController]
[Route("[controller]")]
public class FilesController : Controller
{
    private readonly IMediator _mediator;
    private readonly IUserService _userService;

    /// <summary>
    /// Constructor lol
    /// </summary>
    /// <param name="mediator">Standard MediatR interface to interact with application layer</param>
    /// <param name="userService">Interface to get authorized users</param>
    public FilesController(IMediator mediator, IUserService userService)
    {
        _mediator = mediator;
        _userService = userService;
    }

    /// <summary>
    /// Add track to service in user's profile
    /// </summary>
    /// <param name="token">Token to specify and verify user</param>
    /// <param name="form"></param>
    /// <param name="cancellationToken">CancellationToken to observe while waiting for the task to complete</param>
    /// <returns>Identification descriptor for added track</returns>
    [HttpPost("track")]
    public async Task<ActionResult<UploadTrack.Response>> UploadTrackAsync(
        [FromHeader(Name = "Token")] string token,
        [FromForm] TrackFormDto form,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        await using var fileStream = form.File.OpenReadStream();
        
        return Ok(
            await _mediator.Send(
                new UploadTrack.Command(
                    user, 
                    form.Name, 
                    new UploadTrack.Command.TrackFile(
                        form.File.FileName, 
                        fileStream)
                    ), cancellationToken));
    }

    /// <summary>
    /// Metainformation to setup streaming via m3u8 format
    /// </summary>
    /// <param name="token">Token to specify and verify user</param>
    /// <param name="trackId">Identification descriptor for streamed track</param>
    /// <param name="cancellationToken">CancellationToken to observe while waiting for the task to complete</param>
    /// <returns>File with required information</returns>
    [HttpGet("track-stream-info")]
    public async Task<IActionResult> GetTrackStreamInfoAsync(
        [FromHeader(Name = "Token")] string token,
        [FromQuery(Name = "id")] Guid trackId, 
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetTrackStreamInfo.Query(token, trackId), cancellationToken);
        return File(response.TrackInfoStream, "application/x-mpegURL", true);
    }

    /// <summary>
    /// Get small part to uniform load distribution of streaming track
    /// </summary>
    /// <param name="token">Token to specify and verify user</param>
    /// <param name="streamPartName">Name of part to stream next</param>
    /// <param name="cancellationToken">CancellationToken to observe while waiting for the task to complete</param>
    /// <returns>File with required information</returns>
    [HttpGet("{streamPartName}")]
    public async Task<IActionResult> GetStreamPartAsync(
        [FromHeader(Name = "Token")] string token,
        [FromRoute] string streamPartName,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserAsync(token, cancellationToken);
        var response = await _mediator.Send(new GetStreamPart.Query(streamPartName), cancellationToken);
        return File(response.StreamPart, "audio/MPA", true);
    }

    /// <summary>
    /// Remove track from user's profile
    /// </summary>
    /// <param name="token">Token to specify and verify user</param>
    /// <param name="trackId">Identification descriptor for removed track</param>
    /// <param name="cancellationToken">CancellationToken to observe while waiting for the task to complete</param>
    /// <returns>Nothing</returns>
    [HttpDelete("track")]
    public async Task<IActionResult> DeleteTrackAsync(
        [FromHeader(Name = "Token")] string token, 
        [FromQuery] Guid trackId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTrack.Command(token, trackId), cancellationToken);
        return Ok();
    }
}