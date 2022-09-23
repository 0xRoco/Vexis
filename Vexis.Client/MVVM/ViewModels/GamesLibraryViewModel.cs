using System.Collections.ObjectModel;
using Vexis.Client.Data;
using Vexis.Client.MVVM.Models;
using Vexis.Common.WPF;

namespace Vexis.Client.MVVM.ViewModels;

public class GamesLibraryViewModel : ViewModelBase<GamesLibraryModel>
{
    public string Search
    {
        get => Model.Search;
        set
        {
            Model.Search = value;
            OnPropertyChanged(Search);
        }
    }

    public ObservableCollection<Game> Games
    {
        get => Model.Games;
        set
        {
            Model.Games = value;
            OnPropertyChanged();
        }
    }
}