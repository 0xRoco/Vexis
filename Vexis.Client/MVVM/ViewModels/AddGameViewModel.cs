using Vexis.Client.Data;
using Vexis.Client.MVVM.Models;
using Vexis.Common.WPF;

namespace Vexis.Client.MVVM.ViewModels;

public class AddGameViewModel : ViewModelBase<AddGameModel>
{
    public Game Game
    {
        get => Model.Game;
        set
        {
            Model.Game = value;
            OnPropertyChanged();
        }
    }
}