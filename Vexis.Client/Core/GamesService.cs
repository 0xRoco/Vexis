using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using badLogg.Core;
using Newtonsoft.Json;
using Vexis.Client.Data;
using Vexis.Client.Data.Enums;
using Vexis.Common;

namespace Vexis.Client.Core;

internal sealed class GamesService : LazySingletonBase<GamesService>
{
    
    public bool IsGameRunning { get; private set; }
    public Game? CurrentGame { get; private set; }
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
            Logger.Error($"Error loading custom games: {e.GetBaseException()}");
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
            Logger.Error($"Failed to save custom games: {e.GetBaseException()}");
        }
    }

    public async Task LaunchGame(Game game)
    {
        try
        {
            if (IsGameRunning) return;
            foreach (var g in Games.Where(g => game == g))
            {
                if (g.GameLauncher is CustomGameLauncher.None)
                {
                    var process = await StartProcess(game);
                    if (process == null) continue;
                    Logger.Debug($"{game} - Launching: {!process.HasExited}");
                    IsGameRunning = true;
                    CurrentGame = game;
                }
                else
                {
                    Logger.Warn($"Launching games from other platforms is not yet supported.");
                    return;
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error($"Failed to launch ({game}): {e.GetBaseException()}");
        }
    }

    private Task<Process?> StartProcess(Game game)
    {
        try
        {
            var proc = new Process();
            proc.StartInfo.FileName = $"{game.GameDirectory}/{game.GameExecutable}";
            proc.StartInfo.WorkingDirectory = game.GameDirectory;
            proc.StartInfo.Arguments = game.LaunchArgs?.First();
            proc.EnableRaisingEvents = true;
            proc.Exited += (sender, args) => OnProcessExited(sender, args, game);
            proc.Start();
            return Task.FromResult<Process?>(proc);
        }
        catch (Exception e)
        {
            Logger.Error($"Failed to launch process ({game}): {e.GetBaseException()}");
            throw;
        }
    }

    private void OnProcessExited(object? sender, EventArgs e, Game game)
    {
        var proc = sender as Process;
        IsGameRunning = false;
        CurrentGame = null;
        if (proc == null)
        {
            Logger.Debug($"{game.Name} exited");
            return;
        }
        Logger.Debug($"{game.Name} (PID: {proc.Id}) exited ({proc.ExitCode})");
    }

    /* 
    private Task<Game?> UpdateGame(int gameId, Game game)
    {
        var g = Games.FirstOrDefault(x => x.Id == gameId);

        var index = Games.IndexOf(g);
        if (index != -1)
        {
            Games[index] = game;
            Logger.Info($"Updated game");
        }
        
        return Task.FromResult(g)!;
    }
    */
}