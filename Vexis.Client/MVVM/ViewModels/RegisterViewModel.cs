using System.Security;
using Vexis.Client.Core;
using Vexis.Client.MVVM.Models;

namespace Vexis.Client.MVVM.ViewModels;

public class RegisterViewModel : ViewModelBase<RegisterModel>
{
    public string Username
    {
        get => Model.Username;
        set
        {
            Model.Username = value;
            OnPropertyChanged(nameof(Username));
        }
    }

    public string Email
    {
        get => Model.Email;
        set
        {
            Model.Email = value;
            OnPropertyChanged(nameof(Email));
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
}