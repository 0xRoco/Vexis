using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    private string GamesFilePath => Path.Combine(Environment.CurrentDirectory, "data/games.json");

    public GamesService()
    {
        Logger = LogManager.GetLogger();
    }

    public async Task Initialize()
    {
        Logger.Info("GamesService initialized");
        await LoadCustomGames();
        await Task.CompletedTask;
    }

    public async Task AddCustomGame(Game game)
    {
        Logger.Debug($"Adding game: {game.ToJson()}");
        Games.Add(game);
        await UiService.Instance.AddGameTile(game);
        await SaveCustomGames();
    }

    public async Task LoadCustomGames()
    {
        try
        {
            Logger.Info("Loading custom games");
            if (!File.Exists(GamesFilePath))
            {
                Logger.Info("No custom games found");
                return;
            }

            var games = await File.ReadAllTextAsync(GamesFilePath);
            var customGames = JsonConvert.DeserializeObject<List<Game>>(games);
            if (customGames != null)
                foreach (var game in customGames)
                {
                    Games.Add(game);
                    await UiService.Instance.AddGameTile(game);
                }

            Logger.Info("Custom games loaded");
        }
        catch (Exception e)
        {
            Logger.Error($"Error loading custom games: {e.Message}");
        }
    }

    public async Task SaveCustomGames()
    {
        try
        {
            Logger.Info("Saving custom games");
            if (!Directory.Exists(Path.GetDirectoryName(GamesFilePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(GamesFilePath)!);
            var games = JsonConvert.SerializeObject(Games, Formatting.Indented);
            await File.WriteAllTextAsync(GamesFilePath, games);
            Logger.Info("Custom games saved");
        }
        catch (Exception e)
        {
            Logger.Error($"Failed to save custom games: {e.Message}");
        }
    }
}