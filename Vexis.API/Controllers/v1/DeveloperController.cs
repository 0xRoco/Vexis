using Microsoft.AspNetCore.Mvc;
using Vexis.API.Models;

namespace Vexis.API.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class DeveloperController : Controller
{
    [HttpPost("upload-build")]
    [DisableRequestSizeLimit]
    [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
    public async Task<IActionResult> UploadBuild([FromForm] BuildUploadModel model) //TODO: BuildUploadModel should contain some data so we can verify the developer
    {
        if (model.File.Length > 0)
        {
            Console.WriteLine(model.File.FileName);
            var filepath = Path.Combine(Environment.CurrentDirectory, $"Games/{model.File.FileName}");
            await using var stream = System.IO.File.Create(filepath);
            await model.File.CopyToAsync(stream);
        }
        else
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        return StatusCode(200);
    }
    
    //TODO: This should take gameId and buildId
    [HttpGet("download-build")]
    public async Task<FileResult> DownloadBuild()
    {
        
        // physicalPath should point to Games/[gameId]/[buildId]/[gameName].zip
        // fileDownloadName would be [gameName].zip
        return PhysicalFile("$CHANGE_ME", "application/zip", "$CHANGE_ME");
    }
}