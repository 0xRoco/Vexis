using System.Windows;
using System.Windows.Controls;
using badLogg.Core;
using Vexis.Client.Data;
using Vexis.Client.MVVM.ViewModels;

namespace Vexis.Client.MVVM.Controls;

public partial class GameTile : UserControl
{
    private GameTileViewModel ViewModel { get; set; } = new();

    public GameTile()
    {
        InitializeComponent();
    }

    private void PlayButton_OnClick(object sender, RoutedEventArgs e)
    {
        LogManager.GetLogger().Info(ViewModel.Game.ToString());
    }

    private void GameTile_OnLoaded(object sender, RoutedEventArgs e)
    {
        ViewModel.Game = (Game) DataContext;
        DataContext = ViewModel;
    }
}