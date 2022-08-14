using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using badLogg.Core;
using Newtonsoft.Json;
using Vexis.Client.Data.Enums;

namespace Vexis.Client.Data;

public class Game
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string GameExecutable { get; set; }
    public string GameDirectory { get; set; }
    public List<string>? LaunchArgs { get; set; }

    public DateTime LastPlayed { get; set; } = DateTime.MinValue;
    public TimeSpan Playtime { get; set; } = TimeSpan.Zero;

    public bool IsCustomGame { get; set; }

    public CustomGameLauncher GameLauncher { get; set; }

    public string GameLauncherIcon => GetGameLauncherIcon();


    public Game(int id, string name, string gameExecutable, string gameDirectory, bool isCustomGame = false,
        CustomGameLauncher gameLauncher = CustomGameLauncher.None,
        List<string>? launchArgs = null)
    {
        if (name.EndsWith(".exe"))
            name = name[..^4];

        Id = id;
        Name = name;
        GameExecutable = gameExecutable;
        GameDirectory = gameDirectory;
        IsCustomGame = isCustomGame;
        LaunchArgs = launchArgs;
        GameLauncher = gameLauncher;
    }

    public override string ToString()
    {
        return $"{Name} ({Id})";
    }

    private string GetGameLauncherIcon()
    {
        return GameLauncher switch
        {
            CustomGameLauncher.None => "Play",
            CustomGameLauncher.Steam => "Steam",
            CustomGameLauncher.EpicGames => "Epic Games",
            CustomGameLauncher.BattleNet => "Battle.net",
            _ => "Play"
        };
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}