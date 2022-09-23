using Vexis.Client.MVVM.Models;
using Vexis.Common.WPF;

namespace Vexis.Client.MVVM.ViewModels;

public class GameActivationViewModel : ViewModelBase<GameActivationModel>
{
    public string GameCode
    {
        get => Model.GameCode;
        set
        {
            Model.GameCode = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsCodeValid));
        }
    }

    public bool IsCodeValid => CheckCode();


    private bool CheckCode()
    {
        if (string.IsNullOrEmpty(GameCode)) return false;
        return true;
    }
}