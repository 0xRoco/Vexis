using System.Windows;
using System.Windows.Controls;
using badLogg.Core;
using Vexis.Client.MVVM.ViewModels;

namespace Vexis.Client.MVVM.Controls;

public partial class GameActivation : UserControl
{
    private GameActivationViewModel ViewModel { get; set; } = new();

    public GameActivation()
    {
        InitializeComponent();
    }

    private void GameActivation_OnLoaded(object sender, RoutedEventArgs e)
    {
        ViewModel = new GameActivationViewModel();
        DataContext = ViewModel;
    }

    private void ActivateButton_OnClick(object sender, RoutedEventArgs e)
    {
        LogManager.GetLogger().Debug($"Activating game with code {ViewModel.GameCode}");
        ViewModel.GameCode = string.Empty;
    }
}