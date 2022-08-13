using System.Collections.Generic;
using System.Collections.ObjectModel;
using Vexis.Client.Data;

namespace Vexis.Client.MVVM.Models;

public class GamesLibraryModel
{
    public string Search { get; set; }
    public ObservableCollection<Game> Games { get; set; } = new();
}