using Vexis.Client.Data;
using Vexis.Client.MVVM.Models;
using Vexis.Common.WPF;

namespace Vexis.Client.MVVM.ViewModels;

internal class MainWindowViewModel : ViewModelBase<MainWindowModel>
{
    public CurrentUser User
    {
        get => Model.User;
        set
        {
            Model.User = value;
            OnPropertyChanged();
        }
    }

    public AppClient AppClient
    {
        get => Model.AppClient;
        set
        {
            Model.AppClient = value;
            OnPropertyChanged();
        }
    }
}