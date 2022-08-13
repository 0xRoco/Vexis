﻿namespace Vexis.Client.Data
{
    internal class ClientSettings
    {
        public bool RunOnStartup { get; set; }
        public bool IsUserAlreadyLoggedIn { get; set; }
        public string UserLoginToken { get; set; } = "";
    }
}