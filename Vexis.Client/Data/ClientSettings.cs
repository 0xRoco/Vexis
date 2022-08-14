using Vexis.API.Data;

namespace Vexis.Client.Data;

internal class ClientSettings
{
    public bool RunOnStartup { get; set; }
    public bool IsUserAlreadyLoggedIn { get; set; }
    public string Username { get; set; }
    public string LoginToken { get; set; }
}