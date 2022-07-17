// See https://aka.ms/new-console-template for more information
string path = @"C:\Users\aless\source\repos";
List<string> projectNames = new() { "RepositoryFramework", "RystemV3", "Rystem.Concurrency", "Rystem.BackgroundJob", "Rystem.Queue" };

var rystemDirectories = new DirectoryInfo(path).GetDirectories().Where(x => projectNames.Contains(x.Name)).ToList();

foreach (var rystemDirectory in rystemDirectories)
{
    
}
