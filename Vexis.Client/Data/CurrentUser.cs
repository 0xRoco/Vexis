using Vexis.Client.Data.Enums;

namespace Vexis.Client.Data;

public class CurrentUser
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string ProfilePictureUrl { get; set; }
    public string LoginToken { get; set; } // clear after login/logout
    public bool IsEmailVerified { get; set; }
    public bool IsPhoneNumberVerified { get; set; }
    public CurrentUserState State { get; set; }


    public override string ToString()
    {
        return $"{Username} ({Email}))";
    }
}