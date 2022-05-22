using Microsoft.AspNetCore.Mvc;

namespace Sonar.Player.Api.Controllers;

[ApiController]
[Route("files")]
public class FilesController
{
    [HttpPost("/track")]
    public async Task<IActionResult> UploadTrackAsync([FromQuery] string name)
    {
        //TODO: get file from request content
        throw new NotImplementedException();
    }

    [HttpGet("/trackStreamInfo")]
    public async Task<IActionResult> GetTrackStreamInfoAsync([FromQuery(Name = "id")] string trackId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("/{streamPartName}")]
    public async Task<IActionResult> GetStreamPartAsync(string streamPartName)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("/track}")]
    public async Task<IActionResult> DeleteTrackAsync([FromQuery] string trackId)
    {
        throw new NotImplementedException();
    }
}