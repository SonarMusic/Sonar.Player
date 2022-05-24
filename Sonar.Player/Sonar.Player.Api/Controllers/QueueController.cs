using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sonar.Player.Application.Queue.Commands;
using Sonar.Player.Application.Queue.Queries;

namespace Sonar.Player.Api.Controllers;

[ApiController]
[Route("{controller}")]
public class QueueController : Controller
{
    private readonly IMediator _mediator;

    public QueueController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<GetQueue.Response>> GetQueueAsync(CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetQueue.Query(), cancellationToken));
    }

    [HttpPatch("/track")]
    public async Task<ActionResult<AddTrackToQueue.Response>> AddTrackToQueueAsync([FromQuery] Guid trackId, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new AddTrackToQueue.Command(), cancellationToken));
    }

    [HttpDelete]
    public async Task<ActionResult<ShuffleQueue.Response>> PurgeQueueAsync(CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new PurgeQueue.Command(), cancellationToken));
    }

    [HttpPatch("/shuffle")]
    public async Task<ActionResult<ShuffleQueue.Response>> ShuffleQueueAsync(CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new ShuffleQueue.Command(), cancellationToken));
    }
}