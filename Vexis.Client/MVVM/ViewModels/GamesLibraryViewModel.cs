using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using Vexis.Client.Core;
using Vexis.Client.Data;
using Vexis.Client.MVVM.Models;

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
            OnPropertyChanged(nameof(Games));
        }
    }
}