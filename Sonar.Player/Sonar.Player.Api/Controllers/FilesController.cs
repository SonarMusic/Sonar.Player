using Microsoft.AspNetCore.Mvc;

namespace Sonar.Player.Api.Controllers;

[ApiController]
[Route("{controller}")]
public class FilesController : Controller
{
    [HttpPost("/track")]
    //TODO: Change to ActionResult<UploadTrackCommand.Response> or sth
    public async Task<IActionResult> UploadTrackAsync([FromQuery] string name)
    {
        //TODO: get file from request content
        throw new NotImplementedException();
    }

    [HttpGet("/trackStreamInfo")]
    public async Task<IActionResult> GetTrackStreamInfoAsync([FromQuery(Name = "id")] Guid trackId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("/{streamPartName}")]
    public async Task<IActionResult> GetStreamPartAsync(string streamPartName)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("/track")]
    public async Task<IActionResult> DeleteTrackAsync([FromQuery] Guid trackId)
    {
        throw new NotImplementedException();
    }
}