using System.Security;
using Vexis.Client.Core;

namespace Vexis.Client.MVVM.Models;

public class LoginPageModel
{
    public string AppName = ClientService.Instance.GetCurrentClient().AppName;
    public string UsernameOrEmail { get; set; }
    public SecureString Password { get; set; }

    public bool RememberMe { get; set; }
}