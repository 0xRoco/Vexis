using System;
using System.ComponentModel;
using System.Windows;
using badLogg.Core;
using Vexis.Client.Core;
using Vexis.Client.Data;
using Vexis.Client.MVVM.Pages;
using Vexis.Client.MVVM.ViewModels;

namespace Vexis.Client.MVVM.Views;

public partial class MainWindow : Window
{
    private MainWindowViewModel ViewModel { get; set; }
    private GamesLibrary GamesLibrary { get; set; } = new();

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new MainWindowViewModel();
        DataContext = ViewModel;
    }

    private void LibraryButton_Click(object sender, RoutedEventArgs e)
    {
    }

    private void StoreButton_Click(object sender, RoutedEventArgs e)
    {
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        iFrame.Content = GamesLibrary;
        ViewModel.User = ClientService.Instance.GetCurrentUser();
        await GamesService.Instance.Initialize();
    }

    public GamesLibrary GetGamesLibrary()
    {
        return GamesLibrary;
    }

    private async void MainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        await ClientService.Instance.SaveClientSettings();
    }
}