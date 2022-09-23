using System.Security;
using Vexis.Client.MVVM.Models;
using Vexis.Common.WPF;

namespace Vexis.Client.MVVM.ViewModels;

public class RegisterPageViewModel : ViewModelBase<RegisterPageModel>
{
    public string AppName
    {
        get => Model.AppName;
        set
        {
            Model.AppName = value;
            OnPropertyChanged();
        }
    }

    public string Username
    {
        get => Model.Username;
        set
        {
            Model.Username = value;
            OnPropertyChanged();
        }
    }

    public string Email
    {
        get => Model.Email;
        set
        {
            Model.Email = value;
            OnPropertyChanged();
        }
    }

    public SecureString Password
    {
        get => Model.Password;
        set
        {
            Model.Password = value;
            OnPropertyChanged();
        }
    }
}