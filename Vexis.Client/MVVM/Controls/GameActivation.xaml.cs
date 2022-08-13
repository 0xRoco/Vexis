using System.Windows;
using System.Windows.Controls;
using badLogg.Core;
using Vexis.Client.MVVM.ViewModels;

namespace Vexis.Client.MVVM.Controls;

public partial class GameActivation : UserControl
{
    private GameActivationViewModel _vm = new();

    public GameActivation()
    {
        InitializeComponent();
    }

    private void GameActivation_OnLoaded(object sender, RoutedEventArgs e)
    {
        _vm = new GameActivationViewModel();
        DataContext = _vm;
    }

    private void ActivateButton_OnClick(object sender, RoutedEventArgs e)
    {
        LogManager.GetLogger().Debug($"Activating game with code {_vm.GameCode}");
        _vm.GameCode = string.Empty;
    }
}