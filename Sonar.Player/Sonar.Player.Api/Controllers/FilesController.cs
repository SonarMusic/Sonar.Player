using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sonar.Player.Application.Files.Commands;
using Sonar.Player.Application.Files.Queries;

namespace Sonar.Player.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FilesController : Controller
{
    private readonly IMediator _mediator;

    public FilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("track")]
    public async Task<ActionResult<UploadTrack.Response>> UploadTrackAsync([FromQuery] string name)
    {
        //TODO: get file from request content
        return Ok(await _mediator.Send(new UploadTrack.Command()));
    }

    [HttpGet("trackStreamInfo")]
    public async Task<IActionResult> GetTrackStreamInfoAsync([FromQuery(Name = "id")] Guid trackId)
    {
        var response = await _mediator.Send(new GetTrackStreamInfo.Query());
        return File(response.TrackInfoStream, "application/x-mpegURL", true);
    }

    [HttpGet("{streamPartName}")]
    public async Task<IActionResult> GetStreamPartAsync([FromRoute] string streamPartName)
    {
        var response = await _mediator.Send(new GetStreamPart.Query());
        return File(response.StreamPart, "audio/MPA", true);
    }

    [HttpDelete("track")]
    public async Task<ActionResult<DeleteTrack.Response>> DeleteTrackAsync([FromQuery] Guid trackId)
    {
        return Ok(await _mediator.Send(new DeleteTrack.Command()));
    }
}