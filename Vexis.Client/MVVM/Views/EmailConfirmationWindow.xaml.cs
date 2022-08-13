using System.ComponentModel;
using System.Windows;

namespace Vexis.Client.MVVM.Views;

public partial class EmailConfirmationWindow : Window
{
    public EmailConfirmationWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = true;
    }

    private void VerifyButton_OnClick(object sender, RoutedEventArgs e)
    {
    }
}