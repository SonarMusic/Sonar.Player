using Microsoft.AspNetCore.Mvc;

namespace Sonar.Player.Api.Controllers;

public class PlayerContextController
{
    [HttpGet("/queue")]
    public async Task<IActionResult> GetQueueAsync()
    {
        throw new NotImplementedException();
    }

    [HttpPatch("/queue/track")]
    public async Task<IActionResult> AddTrackToQueueAsync([FromQuery] string trackId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("/queue")]
    public async Task<IActionResult> PurgeQueueAsync()
    {
        throw new NotImplementedException();
    }

    [HttpPatch("/queue/shuffle")]
    public async Task<IActionResult> ShuffleQueueAsync()
    {
        throw new NotImplementedException();
    }
}