using System;
using System.Threading.Tasks;
using badLogg.Core;
using Vexis.Client.Data;
using Vexis.Client.MVVM.Pages;
using Vexis.Client.MVVM.Views;

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

    public static async Task AddGameTile(Game game)
    {
        if (await WindowsService.Instance.GetWindowAsync("MainWindow") is not MainWindow mainWindow)
        {
            LogManager.GetLogger().Error("MainWindow is null");
            return;
        }

        if (mainWindow.iFrame.Content is not GamesLibrary lib)
        {
            LogManager.GetLogger().Error("Could not find GamesLibrary in MainWindow");
            return;
        }

        lib._vm.Games.Add(game);
    }


    [Obsolete("For testing - remove later")]
    public static async Task CloseCurrentDialogHost()
    {
        LogManager.GetLogger().Debug("Closing dialog host");

        if (await WindowsService.Instance.GetWindowAsync("MainWindow") is not MainWindow mainWindow)
        {
            LogManager.GetLogger().Error("MainWindow is null");
            return;
        }

        mainWindow.DialogHost.IsOpen = false;
    }
}