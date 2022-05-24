using Microsoft.AspNetCore.Mvc;

namespace Sonar.Player.Api.Controllers;

[ApiController]
[Route("{controller}")]
public class QueueController : Controller
{
    [HttpGet]
    //TODO: Change to ActionResult<GetQueueQuery.Response> or sth
    public async Task<IActionResult> GetQueueAsync()
    {
        throw new NotImplementedException();
    }

    [HttpPatch("/track")]
    public async Task<IActionResult> AddTrackToQueueAsync([FromQuery] Guid trackId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public async Task<IActionResult> PurgeQueueAsync()
    {
        throw new NotImplementedException();
    }

    [HttpPatch("/shuffle")]
    public async Task<IActionResult> ShuffleQueueAsync()
    {
        throw new NotImplementedException();
    }
}