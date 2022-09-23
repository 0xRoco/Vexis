// See https://aka.ms/new-console-template for more information

using Vexis.DevAccess.CLI;

Console.WriteLine("zip path: ");
var zipPath = Console.ReadLine();
if (zipPath != null) await BuildManager.UploadBuild(1, "Test", zipPath);