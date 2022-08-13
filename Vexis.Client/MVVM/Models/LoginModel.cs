using System.Security;

namespace Vexis.Client.MVVM.Models;

public class LoginModel
{
    public string UsernameOrEmail { get; set; }
    public SecureString Password { get; set; }

    public bool RememberMe { get; set; }
}