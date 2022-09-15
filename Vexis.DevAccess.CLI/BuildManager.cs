using RestSharp;

namespace Vexis.DevAccess.CLI;

public static class BuildManager
{
    public static async Task UploadBuild(int devId, string gameName,string zipPath)
    {
        var client = new RestClient("http://localhost:5000/api/v1/");
        var request = new RestRequest("developer/upload-build", Method.Post){RequestFormat = DataFormat.Json, AlwaysMultipartFormData = true};
        request.AddFile("File", zipPath);
        await client.PostAsync(request);
    }
}