using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using badLogg.Core;
using Microsoft.Win32;
using Vexis.Client.Core;
using Vexis.Client.Data;
using Vexis.Client.Data.Enums;
using Vexis.Client.MVVM.Pages;
using Vexis.Client.MVVM.ViewModels;

namespace Vexis.Client.MVVM.Controls;

public partial class AddGame : UserControl
{
    private AddGameViewModel _vm = new();

    public AddGame()
    {
        InitializeComponent();
    }

    private void AddGame_OnLoaded(object sender, RoutedEventArgs e)
    {
        _vm = new AddGameViewModel();
        DataContext = _vm;
        AddButton.IsEnabled = false;
    }


    //TODO: Make this a method in UiService
    private void BrowseButton_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Executable files (*.exe)|*.exe",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };
            if (dialog.ShowDialog() == true)
            {
                _vm.Game = new Game(0, dialog.SafeFileName, dialog.SafeFileName,
                    Path.GetDirectoryName(dialog.FileName)!);
                AddButton.IsEnabled = true;
                return;
            }

            AddButton.IsEnabled = false;
        }
        catch (Exception ex)
        {
            LogManager.GetLogger().Error($"Error opening file dialog: {ex.Message}");
            AddButton.IsEnabled = false;
        }
    }


    //This is probably the worst way to handle this but who cares?
    private async void AddButton_OnClick(object sender, RoutedEventArgs e)
    {
        await GamesService.Instance.AddGame(_vm.Game);
        AddButton.IsEnabled = false;
        _vm.Game = null;
        await UiService.CloseCurrentDialogHost();
    }
}