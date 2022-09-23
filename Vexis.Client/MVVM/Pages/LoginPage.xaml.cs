using System;
using System.Windows;
using System.Windows.Controls;
using badLogg.Core;
using Vexis.API.Data;
using Vexis.Client.Core;
using Vexis.Client.Data;
using Vexis.Client.MVVM.ViewModels;
using Vexis.Client.MVVM.Views;
using Vexis.Security;

namespace Vexis.Client.MVVM.Pages;

/// <summary>
/// Interaction logic for LoginPage.xaml
/// </summary>
public partial class LoginPage
{
    private LogManager Logger { get; }
    private LoginPageViewModel ViewModel { get; }

    public LoginPage()
    {
        InitializeComponent();
        DataContext = ViewModel = new LoginPageViewModel();
        Logger = LogManager.GetLogger();
    }

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext == null) return;
        {
            ViewModel.Password = ((PasswordBox) sender).SecurePassword;
        }
    }

    private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            LoginButton.IsEnabled = false;
            var x = await AuthService.Instance.Auth(new AuthModel()
            {
                UsernameOrEmail = ViewModel.UsernameOrEmail,
                Password = SecurityService.SecureStringToString(ViewModel.Password)
            });

            if (string.IsNullOrEmpty(x)) return;
            await WindowsService.Instance.HideWindowAsync(nameof(AuthWindow), true);
            ClientService.Instance.CurrentUser.Username = ViewModel.UsernameOrEmail;
            ClientService.Instance.CurrentUser.LoginToken = x;
            if (ViewModel.RememberMe)
                await ClientService.Instance.UpdateSettings(new ClientSettings
                {
                    StartupAction = ClientService.Instance.Settings.StartupAction,
                    GameLaunchAction = ClientService.Instance.Settings.GameLaunchAction,
                    MasterApi = ClientService.Instance.Settings.MasterApi,
                    LoginToken = $"{ViewModel.UsernameOrEmail}:{x}"
                });

            await WindowsService.Instance.CreateWindowAsync(nameof(InitializingWindow));
            await WindowsService.Instance.ShowWindowAsync(nameof(InitializingWindow));
        }
        catch (Exception ex)
        {
            Logger.Error(ex.GetBaseException().ToString());
        }
        finally
        {
            ResetFields();
        }
    }

    private void ResetFields()
    {
        UsernameTextBox.Clear();
        PasswordBox.Clear();
        RememberMeCheckBox.IsChecked = false;
        LoginButton.IsEnabled = true;
    }
}