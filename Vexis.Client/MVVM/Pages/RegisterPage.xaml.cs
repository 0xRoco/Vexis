using System;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using Vexis.API.Data;
using Vexis.Client.Core;
using Vexis.Client.MVVM.ViewModels;
using Vexis.Client.MVVM.Views;
using Vexis.Security;

namespace Vexis.Client.MVVM.Pages;

/// <summary>
/// Interaction logic for RegisterPage.xaml
/// </summary>
public partial class RegisterPage
{
    private RegisterViewModel ViewModel { get; }

    public RegisterPage()
    {
        InitializeComponent();
        DataContext = ViewModel = new RegisterViewModel();
    }

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext == null) return;
        ViewModel.Password = ((PasswordBox) sender).SecurePassword;
    }

    private async void RegisterButton_OnClick(object sender, RoutedEventArgs e)
    {
        RegisterButton.IsEnabled = false;
        var success = await AuthService.Instance.Register(new RegisterModel
        {
            Username = ViewModel.Username,
            Password = SecurityService.SecureStringToString(ViewModel.Password),
            Email = ViewModel.Email
        });
        if (success) ResetFields();
    }

    private void ResetFields()
    {
        UsernameTextBox.Clear();
        EmailTextBox.Clear();
        PasswordBox.Clear();
        RegisterButton.IsEnabled = true;
    }
}