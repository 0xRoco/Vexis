using System.Security;
using Vexis.Client.Core;
using Vexis.Client.MVVM.Models;

namespace Vexis.Client.MVVM.ViewModels;

public class LoginViewModel : ViewModelBase<LoginModel>
{
    public string UsernameOrEmail
    {
        get => Model.UsernameOrEmail;
        set
        {
            Model.UsernameOrEmail = value;
            OnPropertyChanged(nameof(UsernameOrEmail));
        }
    }

    public SecureString Password
    {
        get => Model.Password;
        set
        {
            Model.Password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public bool RememberMe
    {
        get => Model.RememberMe;
        set
        {
            Model.RememberMe = value;
            OnPropertyChanged(nameof(RememberMe));
        }
    }
}