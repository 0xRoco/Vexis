namespace Vexis.Client.Data.Enums;

public enum CurrentUserState
{
    None,
    PendingLogin,
    LoggedIn,
    PendingLogout,
    LoggedOut,
    PendingRegistration,
    Registered,
    Error
}