using System;
using System.Threading.Tasks;
using badLogg.Core;
using Vexis.Client.Data;
using Vexis.Client.MVVM.Pages;
using Vexis.Client.MVVM.Views;
using Vexis.Common;

namespace Vexis.Client.Core;

internal sealed class UiService : LazySingletonBase<UiService>
{
    private LogManager Logger { get; }

    public UiService()
    {
        Logger = LogManager.GetLogger();
    }

    public async Task Initialize()
    {
        Logger.Info("UiService initialized");
        await Task.CompletedTask;
    }

    public async Task AddGameTile(Game game)
    {
        if (await WindowsService.Instance.GetWindowAsync("MainWindow") is not MainWindow mainWindow)
        {
            Logger.Error("MainWindow is null");
            return;
        }

        mainWindow.GetGamesLibrary().ViewModel.Games.Add(game);
    }

    public async Task<T?> GetViewModel<T>(string windowName)
    {
        var window = await WindowsService.Instance.GetWindowAsync(windowName);
        if (window != null)
        {
            var viewModel = (T) window.DataContext;
            if (viewModel != null) return viewModel;
        }

        Logger.Warn($"Could not find window or viewmodel for {windowName} - maybe it's not loaded yet?");
        return default;
    }


    [Obsolete("For testing - remove later")]
    public async Task CloseCurrentDialogHost()
    {
        Logger.Debug("Closing dialog host");

        if (await WindowsService.Instance.GetWindowAsync("MainWindow") is not MainWindow mainWindow)
        {
            LogManager.GetLogger().Error("MainWindow is null");
            return;
        }

        mainWindow.DialogHost.IsOpen = false;
    }
}