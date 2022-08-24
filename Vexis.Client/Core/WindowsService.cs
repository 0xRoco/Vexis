using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using badLogg.Core;

namespace Vexis.Client.Core;

public sealed class WindowsService : LazySingletonBase<WindowsService>
{
    private Dictionary<string, string> WindowDictionary { get; } = new();
    private List<Window> CreatedWindows { get; } = new();

    private LogManager Logger { get; } = LogManager.GetLogger();

    public WindowsService()
    {
        WindowDictionary.Add("MainWindow", "Vexis.Client.MVVM.Views.MainWindow");
        WindowDictionary.Add("AuthWindow", "Vexis.Client.MVVM.Views.AuthWindow");
        WindowDictionary.Add("InitializingWindow", "Vexis.Client.MVVM.Views.InitializingWindow");
        WindowDictionary.Add("EmailConfirmationWindow", "Vexis.Client.MVVM.Views.EmailConfirmationWindow");
        Logger.Info("WindowsService initialized");

        //E:\source\Vexis\Vexis.Client\MVVM\Views\AuthWindow.xaml
    }

    public async Task<Window?> GetWindowAsync(string windowName)
    {
        try
        {
            if (DoesWindowExist(windowName) && IsWindowCreated(windowName))
                return await Task.FromResult(CreatedWindows.First(w =>
                    w.GetType().ToString() == WindowDictionary[windowName]));

            Logger.Warn($"Window {windowName} does not exist or has not been created");
            return null!;
        }
        catch (Exception e)
        {
            Logger.Error($"Error getting window {windowName} - {e.GetBaseException()}");
            return null!;
        }
    }

    public async Task ShowWindowAsync(string windowName, bool isDialog = false)
    {
        try
        {
            if (!DoesWindowExist(windowName))
            {
                Logger.Warn($"Window {windowName} does not exist");
                return;
            }

            if (!IsWindowCreated(windowName))
            {
                Logger.Warn($"Window {windowName} is not created");
                return;
            }

            if (IsWindowOpen(windowName))
            {
                Logger.Warn($"Window {windowName} is already open");
                return;
            }

            var window = await GetWindowAsync(windowName);

            switch (isDialog)
            {
                case false:
                    window?.Show();
                    Logger.Info($"Window {windowName} shown");
                    break;

                case true:
                    window?.ShowDialog();
                    Logger.Info($"Window {windowName} shown as dialog");
                    break;
            }
        }
        catch (Exception e)
        {
            Logger.Error($"Error showing window {windowName} - {e.GetBaseException()}");
        }
    }

    //TODO: Make this asynchronous
    public async Task<bool> CreateWindowAsync(string windowName)
    {
        try
        {
            if (!DoesWindowExist(windowName))
            {
                Logger.Warn($"Window {windowName} does not exist");
                return false;
            }

            if (IsWindowCreated(windowName))
            {
                Logger.Warn($"Window {windowName} is already created");
                return true;
            }

            var windowType = Type.GetType(WindowDictionary[windowName]);
            if (windowType == null)
            {
                Logger.Warn($"Window {windowName} type not found");
                return false;
            }

            if (Activator.CreateInstance(windowType) is not Window window)
            {
                Logger.Warn($"Window {windowName} type not found");
                return false;
            }

            CreatedWindows.Add(window);
            Logger.Info($"Window {windowName} created");
            return true;
        }
        catch (Exception e)
        {
            Logger.Error($"Error creating window {windowName} - {e.GetBaseException()}");
            return false;
        }
    }

    public async Task<bool> HideWindowAsync(string windowName, bool dispose = false)
    {
        try
        {
            if (!DoesWindowExist(windowName))
            {
                Logger.Warn($"Window {windowName} does not exist");
                return false;
            }

            if (!IsWindowCreated(windowName))
            {
                Logger.Warn($"Window {windowName} is not created");
                return false;
            }

            if (!IsWindowOpen(windowName))
            {
                Logger.Warn($"Window {windowName} is not open");
                return false;
            }

            var window = await GetWindowAsync(windowName);
            if (window == null!)
            {
                Logger.Warn($"Window {windowName} is null");
                return false;
            }

            window.Hide();
            if (dispose)
            {
                CreatedWindows.Remove(window);
                window.Close();
                Logger.Info($"Window {windowName} closed & disposed");
            }
            else
            {
                Logger.Info($"Window {windowName} hidden");
            }

            return true;
        }
        catch (Exception e)
        {
            Logger.Error($"Error hiding window {windowName} - {e.GetBaseException()}");
            return false;
        }
    }

    public async Task HideAllWindowsAsync()
    {
        foreach (var window in CreatedWindows) await HideWindowAsync(window.GetType().Name);
    }

    public async Task HideAllWindowsExceptAsync(string windowName)
    {
        foreach (var window in CreatedWindows.Where(window =>
                     window.GetType().ToString() != WindowDictionary[windowName]))
            await HideWindowAsync(window.GetType().Name);
    }

    public bool DoesWindowExist(string windowName)
    {
        return WindowDictionary.ContainsKey(windowName);
    }

    public bool IsWindowCreated(string windowName)
    {
        return CreatedWindows.Any(w => w.GetType().ToString() == WindowDictionary[windowName]);
    }

    public bool IsWindowOpen(string windowName)
    {
        return CreatedWindows.Any(w => w.GetType().ToString() == WindowDictionary[windowName] && w.IsVisible);
    }

    public bool IsAnyWindowOpen()
    {
        return CreatedWindows.Any(w => w.IsVisible);
    }
}