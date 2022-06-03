using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sonar.Player.Api.Controllers.Dto;
using Sonar.Player.Application.Files.Commands;
using Sonar.Player.Application.Files.Queries;
using Sonar.Player.Application.Services;

namespace Sonar.Player.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FilesController : Controller
{
    private readonly IMediator _mediator;
    private readonly IUserService _userService;

    public FilesController(IMediator mediator, IUserService userService)
    {
        _mediator = mediator;
        _userService = userService;
    }

    [HttpPost("track")]
    public async Task<ActionResult<UploadTrack.Response>> UploadTrackAsync(
        [FromHeader(Name = "Token")] string token,
        [FromForm] TrackFormDto form)
    {
        var user = await _userService.GetUserAsync(token);
        await using var fileStream = form.File.OpenReadStream();
        
        return Ok(
            await _mediator.Send(
                new UploadTrack.Command(
                    user, 
                    form.Name, 
                    new UploadTrack.Command.TrackFile(
                        form.File.FileName, 
                        fileStream)
                    )));
    }

    [HttpGet("track-stream-info")]
    public async Task<IActionResult> GetTrackStreamInfoAsync(
        [FromHeader(Name = "Token")] string token,
        [FromQuery(Name = "id")] Guid trackId)
    {
        var response = await _mediator.Send(new GetTrackStreamInfo.Query(token, trackId));
        return File(response.TrackInfoStream, "application/x-mpegURL", true);
    }

    [HttpGet("{streamPartName}")]
    public async Task<IActionResult> GetStreamPartAsync(
        [FromHeader(Name = "Token")] string token,
        [FromRoute] string streamPartName)
    {
        var user = await _userService.GetUserAsync(token);
        var response = await _mediator.Send(new GetStreamPart.Query(streamPartName));
        return File(response.StreamPart, "audio/MPA", true);
    }

    [HttpDelete("track")]
    public async Task<IActionResult> DeleteTrackAsync(
        [FromHeader(Name = "Token")] string token, 
        [FromQuery] Guid trackId)
    {
        await _mediator.Send(new DeleteTrack.Command(token, trackId));
        return Ok();
    }
}