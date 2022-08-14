using System;
using System.IO;
using System.Threading.Tasks;
using badLogg.Core;
using Newtonsoft.Json;
using Vexis.API.Data;
using Vexis.Client.Data;
using Vexis.Client.Data.Enums;

namespace Vexis.Client.Core;

internal sealed class ClientService : LazySingletonBase<ClientService>
{
    private string SettingsFilePath => Path.Combine(Environment.CurrentDirectory, "data/settings.json");
    private bool IsInitialized { get; set; }
    private CurrentUser CurrentUser { get; set; }

    private ApiService ApiService { get; set; } = null!;
    private AppClient Client { get; set; } = null!;
    private ClientSettings Settings { get; set; } = null!;
    private LogManager Logger { get; } = LogManager.GetLogger();

    public async Task InitializeClient(AppClient client)
    {
        if (IsInitialized) await Task.CompletedTask;
        Client = client;
        Logger.Info("Client initialized");
        await InitializeServices();
        IsInitialized = true;
    }

    private async Task InitializeServices()
    {
        if (!IsInitialized) await Task.CompletedTask;
        CurrentUser = new CurrentUser();
        await UiService.Instance.Initialize();

        await AuthService.Instance.Initialize();

        //ApiService = new ApiService("localhost"); //Do i even need this here?

        Logger.Info("Services initialized");
    }

    public AppClient GetCurrentClient()
    {
        return Client;
    }

    public CurrentUser GetCurrentUser()
    {
        return CurrentUser;
    }

    public async Task<bool> LoadUserData()
    {
        var response = await AuthService.Instance.GetUserInfo(CurrentUser.Username, CurrentUser.LoginToken);
        if (response == null!)
        {
            Logger.Warn($"Failed to load user data for {CurrentUser.Username}");
            CurrentUser.State = CurrentUserState.Error;
            return false;
        }

        CurrentUser = new CurrentUser
        {
            Username = response.Username,
            Email = response.Email,
            PhoneNumber = response.PhoneNumber,
            ProfilePictureUrl = response.ProfilePictureUrl,
            LoginToken = CurrentUser.LoginToken,
            IsEmailVerified = response.IsEmailVerified,
            IsPhoneNumberVerified = response.IsPhoneNumberVerified,
            State = CurrentUserState.LoggedIn
        };

        return true;
    }

    public bool IsUserReady()
    {
        return CurrentUser.State == CurrentUserState.LoggedIn && IsInitialized;
    }

    public async Task SaveClientSettings()
    {
        Settings = new ClientSettings
        {
            RunOnStartup = false,
            IsUserAlreadyLoggedIn = false,
            Username = CurrentUser.Username,
            LoginToken = CurrentUser.LoginToken
        };

        if (!Directory.Exists(Path.GetDirectoryName(SettingsFilePath)))
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath)!);
        var settings = JsonConvert.SerializeObject(Settings, Formatting.Indented);
        await File.WriteAllTextAsync(SettingsFilePath, settings);
    }
}