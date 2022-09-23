using System;
using System.ComponentModel;
using System.Windows;
using Vexis.Client.Core;

namespace Vexis.Client.MVVM.Views;

/// <summary>
/// Interaction logic for AuthWindow.xaml
/// </summary>
public partial class AuthWindow : Window
{
    public AuthWindow()
    {
        InitializeComponent();
        MainFrame.Navigate(new Uri("MVVM/Pages/LoginPage.xaml", UriKind.RelativeOrAbsolute));
    }

    protected override async void OnClosing(CancelEventArgs e)
    {
        e.Cancel = true;
        if (WindowsService.Instance.IsAnyWindowOpen())
            await WindowsService.Instance.HideWindowAsync(GetType().Name, true);
    }
}