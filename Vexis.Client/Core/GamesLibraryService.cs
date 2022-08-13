using System;
using System.Threading.Tasks;
using badLogg.Core;
using Vexis.Client.Data;

namespace Vexis.Client.Core;

public sealed class GamesLibraryService : LazySingletonBase<GamesLibraryService>
{
    private LogManager Logger { get; }

    public GamesLibraryService()
    {
        Logger = LogManager.GetLogger();
    }

    public async Task Initialize()
    {
        Logger.Info("GamesLibraryService initialized");
        await Task.CompletedTask;
    }
}