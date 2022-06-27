using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sonar.Player.Api.Controllers.Dto;
using Sonar.Player.Application.Properties.Bitrate.Commands;
using Sonar.Player.Application.Queue.Commands;
using Sonar.Player.Application.Queue.Queries;
using Sonar.Player.Application.Services;
using Sonar.Player.Application.Tools;

namespace Sonar.Player.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SetBitrateController : Controller
{
    private readonly IMediator _mediator;
    private readonly IUserService _userService;

    public SetBitrateController(IMediator mediator, IUserService userService)
    {
        _mediator = mediator;
        _userService = userService;
    }

    [HttpPatch("lowBitrate")]
    public async Task<ActionResult<SetBitrate.Response>> SetLowAsync(
        [FromHeader(Name = "Token")] string token,
        [FromQuery(Name = "id")] Guid trackId,
        [FromForm] TrackFormDto form,
        CancellationToken cancellationToken)
    {
        await using var fileStream = form.File.OpenReadStream();
        return Ok(
            await _mediator.Send(
                new SetBitrate.Command(
                    token,
                    trackId,
                    form.Name, 
                    new SetBitrate.Command.TrackFile(
                        form.File.FileName, 
                        fileStream),
                    (int) BitrateEnum.LowBitrate
                ), cancellationToken));
    }

    [HttpPatch("mediumBitrate")]
    public async Task<ActionResult<SetBitrate.Response>> SetMediumAsync(
        [FromHeader(Name = "Token")] string token,
        [FromQuery(Name = "id")] Guid trackId,
        [FromForm] TrackFormDto form,
        CancellationToken cancellationToken)
    {
        await using var fileStream = form.File.OpenReadStream();
        return Ok(
            await _mediator.Send(
                new SetBitrate.Command(
                    token,
                    trackId,
                    form.Name, 
                    new SetBitrate.Command.TrackFile(
                        form.File.FileName, 
                        fileStream),
                    (int) BitrateEnum.MediumBitrate
                ), cancellationToken));
    }
    
    [HttpPatch("highBitrate")]
    public async Task<ActionResult<SetBitrate.Response>> SetHighAsync(
        [FromHeader(Name = "Token")] string token,
        [FromQuery(Name = "id")] Guid trackId,
        [FromForm] TrackFormDto form,
        CancellationToken cancellationToken)
    {
        await using var fileStream = form.File.OpenReadStream();
        return Ok(
            await _mediator.Send(
                new SetBitrate.Command(
                    token,
                    trackId,
                    form.Name, 
                    new SetBitrate.Command.TrackFile(
                        form.File.FileName, 
                        fileStream),
                    (int) BitrateEnum.HighBitrate
                ), cancellationToken));
    }
}