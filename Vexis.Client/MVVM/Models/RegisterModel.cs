using System.Security;

namespace Vexis.Client.MVVM.Models;

public class RegisterModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public SecureString Password { get; set; }
}