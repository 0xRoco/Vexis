using System;
using System.IO;
using System.Threading.Tasks;
using badLogg.Core;
using Newtonsoft.Json;
using Vexis.Client.Data;
using Vexis.Client.Data.Enums;
using Vexis.Client.MVVM.Views;

namespace Vexis.Client.Core;

internal sealed class ClientService : LazySingletonBase<ClientService>
{
    public AppClient Client { get; private set; } = null!;
    public CurrentUser CurrentUser { get; private set; } = new();
    public ClientSettings Settings { get; private set; } = new();
    private static string SettingsFilePath => Path.Combine(Environment.CurrentDirectory, "data/settings.json");
    private bool IsInitialized { get; set; }

    private ApiService ApiService { get; set; } = null!;
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
        await UiService.Instance.Initialize();
        await AuthService.Instance.Initialize();

        //ApiService = new ApiService("localhost"); //Do i even need this here?

        Logger.Info("Services initialized");
        await LoadClientSettings();
        if (!string.IsNullOrEmpty(Settings.LoginToken) && !string.IsNullOrEmpty(Settings.Username))
        {
            CurrentUser.State = CurrentUserState.PendingLogin;
            await WindowsService.Instance.CreateWindowAsync(nameof(InitializingWindow));
            await WindowsService.Instance.ShowWindowAsync(nameof(InitializingWindow));
        }
        else
        {
            if (await WindowsService.Instance.CreateWindowAsync("AuthWindow"))
                await WindowsService.Instance.ShowWindowAsync("AuthWindow",
                    true); // anything past this point will not be executed until the window is closed
        }
    }


    public async Task UpdateSettings(ClientSettings settings)
    {
        Settings = settings;
        await SaveClientSettings();
    }

    public async Task<bool> LoadUserData()
    {
        Logger.Info("Loading user data");
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
        Logger.Info($"User data loaded for {CurrentUser.Username}");
        return true;
    }

    public bool IsUserReady()
    {
        return CurrentUser.State is CurrentUserState.PendingLogin or CurrentUserState.LoggedIn && IsInitialized;
    }

    public async Task SaveClientSettings()
    {
        Logger.Info("Saving client settings");

        if (!Directory.Exists(Path.GetDirectoryName(SettingsFilePath)))
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath)!);
        var settings = JsonConvert.SerializeObject(Settings, Formatting.Indented);
        await File.WriteAllTextAsync(SettingsFilePath, settings);

        Logger.Info("Client settings saved");
    }

    public async Task LoadClientSettings()
    {
        Logger.Info("Loading client settings");

        if (!File.Exists(SettingsFilePath))
        {
            Logger.Info("No client settings found");
            await SaveClientSettings();
            return;
        }

        var settings = await File.ReadAllTextAsync(SettingsFilePath);
        if (string.IsNullOrEmpty(settings))
        {
            Logger.Info("No client settings found");
            return;
        }

        Settings = JsonConvert.DeserializeObject<ClientSettings>(settings) ?? throw new InvalidOperationException();
        CurrentUser.Username = Settings.Username;
        CurrentUser.LoginToken = Settings.LoginToken;
        Logger.Info($"Client settings loaded");
    }
}