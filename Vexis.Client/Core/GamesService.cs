using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using badLogg.Core;
using Newtonsoft.Json;
using Vexis.Client.Data;

namespace Vexis.Client.Core;

internal sealed class GamesService : LazySingletonBase<GamesService>
{
    private LogManager Logger { get; }
    private List<Game> Games { get; } = new();

    public GamesService()
    {
        Logger = LogManager.GetLogger();
    }

    public async Task Initialize()
    {
        Logger.Info("GamesService initialized");
        await Task.CompletedTask;
    }

    public async Task AddGame(Game game)
    {
        Logger.Info($"Adding game: {game.ToJson()}");
        await UiService.AddGameTile(game);
    }
}