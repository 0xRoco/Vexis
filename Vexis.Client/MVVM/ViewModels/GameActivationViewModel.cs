using System.Threading.Tasks;
using Vexis.Client.Core;
using Vexis.Client.Data;
using Vexis.Client.Data.Enums;
using Vexis.Client.MVVM.Models;

namespace Vexis.Client.MVVM.ViewModels;

public class GameActivationViewModel : ViewModelBase<GameActivationModel>
{
    public string GameCode
    {
        get => Model.GameCode;
        set
        {
            Model.GameCode = value;
            OnPropertyChanged(nameof(GameCode));
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