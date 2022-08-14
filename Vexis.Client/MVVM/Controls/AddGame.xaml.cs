using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using badLogg.Core;
using Microsoft.Win32;
using Vexis.Client.Core;
using Vexis.Client.Data;
using Vexis.Client.Data.Enums;
using Vexis.Client.MVVM.Pages;
using Vexis.Client.MVVM.ViewModels;
using Vexis.Client.MVVM.Views;

namespace Vexis.Client.MVVM.Controls;

public partial class AddGame
{
    private AddGameViewModel ViewModel { get; set; } = new();

    public AddGame()
    {
        InitializeComponent();
    }

    private void AddGame_OnLoaded(object sender, RoutedEventArgs e)
    {
        ViewModel = new AddGameViewModel();
        DataContext = ViewModel;
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
                ViewModel.Game = new Game(RandomNumberGenerator.GetInt32(00000, 99999), dialog.SafeFileName,
                    dialog.SafeFileName,
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
        await GamesService.Instance.AddCustomGame(ViewModel.Game);
        AddButton.IsEnabled = false;
        ViewModel.Game = null!;
        await UiService.Instance.CloseCurrentDialogHost();
    }
}