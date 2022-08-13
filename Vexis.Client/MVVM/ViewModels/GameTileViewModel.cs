using Vexis.Client.Core;
using Vexis.Client.Data;
using Vexis.Client.MVVM.Models;

namespace Vexis.Client.MVVM.ViewModels;

public class GameTileViewModel : ViewModelBase<GameTileModel>
{
    public Game Game
    {
        get => Model.Game;
        set
        {
            Model.Game = value;
            OnPropertyChanged(nameof(Game));
        }
    }
}