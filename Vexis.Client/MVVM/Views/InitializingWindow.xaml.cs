using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using badLogg.Core;
using Vexis.Client.Core;
using Vexis.Client.Data;

namespace Vexis.Client.MVVM.Views;

public partial class InitializingWindow
{
    private LogManager Logger { get; }

    public InitializingWindow()
    {
        InitializeComponent();
        Logger = LogManager.GetLogger();
    }

    protected override async void OnContentRendered(EventArgs e)
    {
        //setup the app to load user info and load dashboard
        base.OnContentRendered(e);
        while (!ClientService.Instance.IsUserReady())
        {
            Logger.Info("User is not ready, waiting for 5000ms");
            await Task.Delay(5000);
        }

        Logger.Info("User is ready, loading data");
        var dataLoaded = await ClientService.Instance.LoadUserData();
        if (!dataLoaded)
        {
            Logger.Error("Failed to load user data");
            ClientService.Instance.Settings.ClearLoginData();
            await WindowsService.Instance.HideWindowAsync(GetType().Name, true);
            if (await WindowsService.Instance.CreateWindowAsync("AuthWindow"))
                await WindowsService.Instance
                    .ShowWindowAsync(
                        "AuthWindow"); // anything past this point will not be executed until the window is closed
            return;
        }

        Logger.Info("User data loaded, loading dashboard");
        await WindowsService.Instance.HideWindowAsync(GetType().Name, true);
        await ClientService.Instance.SaveClientSettings();
        await WindowsService.Instance.CreateWindowAsync(nameof(MainWindow));
        await WindowsService.Instance.ShowWindowAsync(nameof(MainWindow));
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = true;
    }
}