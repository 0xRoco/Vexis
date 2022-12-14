using System.Threading.Tasks;
using System.Windows;
using badLogg.Core;
using Vexis.Client.Core;
using Vexis.Client.Data;
using Vexis.Client.MVVM.ViewModels;

namespace Vexis.Client.MVVM.Pages;

/// <summary>
///     Interaction logic for GamesLibrary.xaml
/// </summary>
public partial class GamesLibrary
{
    private LogManager Logger { get; }
    public GamesLibraryViewModel ViewModel { get; set; } = new();

    public GamesLibrary()
    {
        InitializeComponent();
        Logger = LogManager.GetLogger();
        DataContext = ViewModel;
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
    }

    private async void GamesLibrary_OnLoaded(object sender, RoutedEventArgs e)
    {
    }
}