namespace Vexis.Client.Data;

internal class AppClient
{
    public string AppName { get; }
    public string AppVersion { get; }
    public ClientSettings ClientSettings { get; }

    public AppClient(string appName, string appVersion, ClientSettings? clientSettings = null)
    {
        AppName = appName;
        AppVersion = appVersion;
        ClientSettings = clientSettings ?? new ClientSettings();
    }
}