using System.Security;
using Vexis.Client.Core;

namespace Vexis.Client.MVVM.Models;

public class RegisterPageModel
{
    public string AppName { get; set; } = ClientService.Instance.GetCurrentClient().AppName;
    public string Username { get; set; }
    public string Email { get; set; }
    public SecureString Password { get; set; }
}