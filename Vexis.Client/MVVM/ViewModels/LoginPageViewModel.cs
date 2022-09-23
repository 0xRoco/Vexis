using System.Security;
using Vexis.Client.MVVM.Models;
using Vexis.Common.WPF;

namespace Vexis.Client.MVVM.ViewModels;

public class LoginPageViewModel : ViewModelBase<LoginPageModel>
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

    public string UsernameOrEmail
    {
        get => Model.UsernameOrEmail;
        set
        {
            Model.UsernameOrEmail = value;
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

    public bool RememberMe
    {
        get => Model.RememberMe;
        set
        {
            Model.RememberMe = value;
            OnPropertyChanged();
        }
    }
}