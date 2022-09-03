using System;
using System.Threading.Tasks;
using badLogg.Core;
using Vexis.API.Data;
using Vexis.Client.Data.Enums;
using Vexis.Common;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Vexis.Client.Core;

public sealed class AuthService : LazySingletonBase<AuthService>
{
    private LogManager Logger { get; set; } = LogManager.GetLogger();

    private ApiService Api { get; } = new("http://localhost:5000");

    public async Task Initialize()
    {
        ClientService.Instance.CurrentUser.State = CurrentUserState.None;
        await Task.CompletedTask;
    }

    public async Task<bool> Register(RegisterModel model)
    {
        try
        {
            ClientService.Instance.CurrentUser.State = CurrentUserState.PendingRegistration;
            var response = await Api.RegisterAsync(model);
            if (response is {Success: true})
            {
                ClientService.Instance.CurrentUser.State = CurrentUserState.Registered;
                return response is {Success: true};
            }

            Logger.Error($"Registration failed: {response.Message}");
            ClientService.Instance.CurrentUser.State = CurrentUserState.Error;
            return false;
        }
        catch (Exception e)
        {
            Logger.Error($"Registration failed: {e.Message}");
            ClientService.Instance.CurrentUser.State = CurrentUserState.Error;
            return false;
        }
    }

    public async Task<string> Auth(AuthModel model)
    {
        try
        {
            var response = await Api.AuthAsync(model);
            if (response is not {Success: true})
            {
                Logger.Error($"Auth failed : {response.Message}");
                ClientService.Instance.CurrentUser.State = CurrentUserState.Error;
                return null!;
            }

            string loginToken = JsonSerializer.Deserialize<string>(response.Data);
            if (loginToken == null)
            {
                Logger.Error($"Auth failed - no token received");
                ClientService.Instance.CurrentUser.State = CurrentUserState.Error;
                return null!;
            }

            ClientService.Instance.CurrentUser.State = CurrentUserState.PendingLogin;

            return loginToken;
        }
        catch (Exception e)
        {
            Logger.Error($"Auth failed - {e.Message}");
            ClientService.Instance.CurrentUser.State = CurrentUserState.Error;
            return null!;
        }
    }

    public async Task<UserModel> GetUserInfo(string username, string loginToken)
    {
        try
        {
            var response = await Api.GetUserAsync(username, loginToken);
            if (response is not {Success: true})
            {
                Logger.Error($"GetUserInfo failed : {response.Message}");
                return null!;
            }

            UserModel userModel = JsonSerializer.Deserialize<UserModel>(response.Data?.ToString()!);
            if (userModel == null)
            {
                Logger.Error($"GetUserInfo failed - no user received");
                return null!;
            }

            ClientService.Instance.CurrentUser.State = CurrentUserState.LoggedIn;
            return userModel;
        }
        catch (Exception e)
        {
            Logger.Error($"{e.Message}");
            return null!;
        }
    }
}