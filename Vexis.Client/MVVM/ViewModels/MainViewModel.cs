﻿using Vexis.API.Data;
using Vexis.Client.Core;
using Vexis.Client.Data;
using Vexis.Client.MVVM.Models;

namespace Vexis.Client.MVVM.ViewModels;

internal class MainViewModel : ViewModelBase<MainModel>
{
    public CurrentUser User
    {
        get => Model.User;
        set
        {
            Model.User = value;
            OnPropertyChanged(nameof(User));
        }
    }

    public AppClient AppClient
    {
        get => Model.AppClient;
        set
        {
            Model.AppClient = value;
            OnPropertyChanged(nameof(AppClient));
        }
    }
}