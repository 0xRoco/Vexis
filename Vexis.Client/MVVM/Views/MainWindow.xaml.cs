using System;
using System.Windows;
using badLogg.Core;
using Vexis.Client.Core;
using Vexis.Client.Data;
using Vexis.Client.MVVM.ViewModels;

namespace Vexis.Client.MVVM.Views;

public partial class MainWindow : Window
{
    private MainViewModel _vm;

    public MainWindow()
    {
        InitializeComponent();
        _vm = new MainViewModel();
        DataContext = _vm;
    }

    private void LibraryButton_Click(object sender, RoutedEventArgs e)
    {
        iFrame.Navigate(new Uri("../MVVM/Pages/GamesLibrary.xaml", UriKind.Relative));
    }

    private void StoreButton_Click(object sender, RoutedEventArgs e)
    {
        //TODO
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _vm.User = ClientService.Instance.GetCurrentUser();
    }
}