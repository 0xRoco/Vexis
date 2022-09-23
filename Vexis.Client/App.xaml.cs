using System;
using System.Reflection;
using System.Windows;
using badLogg.Core;
using Vexis.Client.Core;

namespace Vexis.Client;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private LogManager Logger { get; }

    public App()
    {
        Logger = new LogManager(new LogConfig()
            .SetAppName("Vexis.Client")
            .SetLogDirectory(
                $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Vexis\\Vexis.Client\\Logs")
            .SetMaxLogs(3)
            .WithConsoleLogging()
            .WithFileLogging());
        Logger.CreateConsole();

        //Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
    }

    private async void App_OnStartup(object sender, StartupEventArgs e)
    {
        Logger.Info($"Logging started at {DateTime.Now}");
        Logger.Info($"App root: {Environment.CurrentDirectory}");
        Logger.Info($"Log directory: {Logger.GetConfig().LogDirectory}");
        Logger.Info($"{Logger.GetConfig().AppName} v{Assembly.GetExecutingAssembly().GetName().Version}");

        await ClientService.Instance.InitializeClient(new Data.AppClient("Vexis.Client", "v1.0.0"));
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        Logger.Info("Application exiting");
        await ClientService.Instance.SaveClientSettings();
        base.OnExit(e);
    }
}