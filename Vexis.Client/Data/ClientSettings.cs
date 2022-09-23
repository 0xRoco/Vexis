using Vexis.Client.Data.Enums;

namespace Vexis.Client.Data;

internal class ClientSettings
{
    public SystemStartupAction StartupAction { get; set; }
    public OnGameLaunchAction GameLaunchAction { get; set; }

    public string MasterApi { get; set; } = "";
    public string LoginToken { get; set; } = "";
    

    public void ClearLoginData()
    {
        LoginToken = "";
    }
}