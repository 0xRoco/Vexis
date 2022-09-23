using System.ComponentModel.DataAnnotations;

namespace Vexis.API.Models;

public class BuildUploadModel
{
    public IFormFile File { get; set; }
}