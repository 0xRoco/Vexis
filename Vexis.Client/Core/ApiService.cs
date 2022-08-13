using System;
using System.Text.Json;
using System.Threading.Tasks;
using badLogg.Core;
using Newtonsoft.Json;
using RestSharp;
using Vexis.API.Data;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Vexis.Client.Core;

internal class ApiService
{
    private string Endpoint { get; }
    private int ApiVersion { get; }
    private LogManager Logger { get; }

    public ApiService(string endpoint, int apiVersion = 1)
    {
        Endpoint = endpoint;
        ApiVersion = apiVersion;
        Logger = LogManager.GetLogger();
        Logger.Info($"API Created - BaseAddress: {endpoint}/api/v{apiVersion}/");
    }


    public async Task<ApiResponse> GetUserAsync(string username, string token)
    {
        var client = new RestClient($"{Endpoint}/api/v{ApiVersion}/");
        try
        {
            var request = new RestRequest($"Users/{username}");
            request.AddHeader("loginToken", token);
            var response = await client.GetAsync<ApiResponse>(request);
            if (response != null)
            {
                var rjson = JsonSerializer.Serialize(response, new JsonSerializerOptions {WriteIndented = true});
                Logger.Info($"{rjson}");
                return response;
            }

            Logger.Error("Failed to retrieve user data - Response is null");
            return new ApiResponse("Failed to retrieve user data", false, null);
        }
        catch (Exception e)
        {
            Logger.Error($"{e.Message}");
            return new ApiResponse("Failed to retrieve user data", false, null);
        }
        finally
        {
            client.Dispose();
        }
    }

    public async Task<ApiResponse> RegisterAsync(RegisterModel model)
    {
        var client = new RestClient($"{Endpoint}/api/v{ApiVersion}/");
        try
        {
            var modelJson = JsonConvert.SerializeObject(model, Formatting.Indented);
            Logger.Info($"Registering account {modelJson}");
            var request = new RestRequest("users/register");
            request.AddJsonBody(model);
            var response = await client.PostAsync<ApiResponse>(request);

            if (response != null)
            {
                var rjson = JsonSerializer.Serialize(response, new JsonSerializerOptions {WriteIndented = true});
                Logger.Info($"{rjson}");
                return response;
            }

            Logger.Error($"Registering account failed - response is null");
            return new ApiResponse("Registering account failed", false, null);
        }
        catch (Exception e)
        {
            Logger.Error($"{e.Message}");
            return new ApiResponse("Registering account failed", false, null);
        }
        finally
        {
            client.Dispose();
        }
    }

    public async Task<ApiResponse> AuthAsync(AuthModel model)
    {
        var client = new RestClient($"{Endpoint}/api/v{ApiVersion}/");
        try
        {
            var modelJson = JsonConvert.SerializeObject(model, Formatting.Indented);
            Logger.Info($"Authenticating account {modelJson}");
            var request = new RestRequest("users/auth", Method.Post);
            request.AddJsonBody(model);
            var response = await client.PostAsync<ApiResponse>(request);
            if (response != null)
            {
                var rjson = JsonSerializer.Serialize(response, new JsonSerializerOptions {WriteIndented = true});
                Logger.Info($"{rjson}");
                return response;
            }

            Logger.Error($"Authenticating account failed - response is null");
            return new ApiResponse("Authentication failed", false, null);
        }
        catch (Exception e)
        {
            Logger.Error($"{e.Message}");
            return new ApiResponse("Authentication failed", false, null);
        }
        finally
        {
            client.Dispose();
        }
    }
}