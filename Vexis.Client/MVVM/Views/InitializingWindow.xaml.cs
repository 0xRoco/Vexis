using System;
using System.ComponentModel;
using System.Threading.Tasks;
using badLogg.Core;
using Vexis.Client.Core;

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
            return;
        }

        Logger.Info("User data loaded, loading dashboard");
        await WindowsService.Instance.HideWindowAsync(GetType().Name);
        await WindowsService.Instance.CreateWindowAsync(nameof(MainWindow));
        await WindowsService.Instance.ShowWindowAsync(nameof(MainWindow));
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = true;
    }
}