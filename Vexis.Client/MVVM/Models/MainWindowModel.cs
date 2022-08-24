using Vexis.API.Data;
using Vexis.Client.Core;
using Vexis.Client.Data;

namespace Vexis.Client.MVVM.Models;

internal class MainWindowModel
{
    public CurrentUser User { get; set; }
    public AppClient AppClient { get; set; } = ClientService.Instance.Client;
}