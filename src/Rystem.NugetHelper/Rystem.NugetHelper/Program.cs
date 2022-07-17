// See https://aka.ms/new-console-template for more information
using CliWrap;
using CliWrap.Buffered;
using Rystem.NugetHelper;
using Rystem.NugetHelper.Engine;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management.Automation;
using System.Text;
using System.Text.RegularExpressions;

namespace Rystem.Nuget
{
    public class Program
    {
        static readonly Regex regexForVersion = new("<Version>[^<]*</Version>");
        static Dictionary<string, string> newVersionOfLibraries = new();
        static VersionType Type = VersionType.Patch;
        static readonly Regex PackageReference = new("<PackageReference[^>]*>");
        static readonly Regex Include = new("Include=");
        static readonly Regex VersionRegex = new(@"Version=\""[^\""]*\""");
        static readonly Regex Repo = new(@"\\repos\\");
        const int AddingValueForVersion = 1;
        public static async Task Main()
        {
            string path = @$"{Repo.Split(Directory.GetCurrentDirectory()).First()}\repos";
            List<string> projectNames = new() { "RepositoryFramework", "RystemV3", "Rystem.Concurrency", "Rystem.BackgroundJob", "Rystem.Queue" };
            var rystemDirectories = new DirectoryInfo(path).GetDirectories().Where(x => projectNames.Contains(x.Name)).ToList();
            Console.WriteLine("Only repository (1) or everything (something else)");
            var line = Console.ReadLine();
            Update? currentUpdateTree = line == "1" ? UpdateConfiguration.OnlyRepositoryTree : UpdateConfiguration.UpdateTree;
            while (currentUpdateTree != null)
            {
                var context = new LibraryContext("0.0.0");
                foreach (var updateTree in currentUpdateTree.Libraries)
                {
                    foreach (var rystemDirectory in rystemDirectories)
                    {
                        await ReadInDeepAsync(rystemDirectory, context, currentUpdateTree);
                    }
                }
                Console.WriteLine($"Current major version is {context.Version.V}");
                foreach (var toUpdate in context.RepoToUpdate)
                {
                    Console.WriteLine($"repo to update {toUpdate}");
                    await CommitAndPushAsync(toUpdate, context.Version.V);
                }
                await Task.Delay(5 * 60 * 1000);
                currentUpdateTree = currentUpdateTree.Son;
            }
        }
        static async Task ReadInDeepAsync(DirectoryInfo directoryInfo, LibraryContext context, Update update)
        {
            bool fileFound = false;
            foreach (var file in directoryInfo!.GetFiles())
            {
                if (file.Name.EndsWith(".csproj") && update.Libraries.Any(x => $"{x.NormalizedName}.csproj" == file.Name))
                {
                    var library = update.Libraries.First(x => $"{x.NormalizedName}.csproj" == file.Name);
                    if (!newVersionOfLibraries.ContainsKey(library.LibraryName))
                    {
                        var streamReader = new StreamReader(file.OpenRead());
                        string content = await streamReader.ReadToEndAsync();
                        streamReader.Dispose();
                        if (regexForVersion.IsMatch(content))
                        {
                            var currentVersion = regexForVersion.Match(content).Value;
                            var version = new NugetHelper.Version(regexForVersion.Match(content).Value.Split('>').Skip(1).First().Split('<').First());
                            version.NextVersion(Type, AddingValueForVersion);
                            Console.WriteLine($"{file.Name} from {currentVersion} to {version.V}");
                            content = content.Replace(currentVersion, $"<Version>{version.V}</Version>");
                            newVersionOfLibraries.Add(library.LibraryName, version.V);
                            foreach (var reference in PackageReference.Matches(content).Select(x => x.Value))
                            {
                                var include = Include.Split(reference).Skip(1).First().Trim('"').Split('"').First();
                                if (newVersionOfLibraries.ContainsKey(include))
                                {
                                    if (VersionRegex.IsMatch(reference))
                                    {
                                        var newReference = reference.Replace(VersionRegex.Match(reference).Value, $"Version=\"{newVersionOfLibraries[include]}\"");
                                        content = content.Replace(reference, newReference);
                                    }
                                }
                            }
                            if (context.Version.IsGreater(version))
                                context.Version = version;
                            Console.WriteLine($"{file.FullName} replaced with");
                            Console.WriteLine("------------------------");
                            Console.WriteLine("------------------------");
                            Console.WriteLine(content);
                            Console.WriteLine("------------------------");
                            Console.WriteLine("------------------------");
                            string path = @$"{Repo.Split(file.FullName).First()}\repos\{Repo.Split(file.FullName).Last().Split('\\').First()}";
                            if (!context.RepoToUpdate.Contains(path))
                                context.RepoToUpdate.Add(path);
                            await File.WriteAllTextAsync(file.FullName, content);
                        }
                    }
                    fileFound = true;
                    break;
                }
            }
            if (!fileFound)
                foreach (var directory in directoryInfo.GetDirectories().Where(x => x.Name != "bin" && x.Name != "obj"))
                {
                    await ReadInDeepAsync(directory, context, update);
                }
        }
        static async Task CommitAndPushAsync(string path, string newVersion)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    WorkingDirectory = path
                },
            };

            process.Start();
            using (var writer = process.StandardInput)
            {
                writer.WriteLine("git init");
                writer.WriteLine("git add .");
                writer.WriteLine($"git commit --author=\"alessandro rapiti <alessandro.rapiti44@gmail.com>\" -m \"new version v.{newVersion}\"");
                writer.WriteLine("git push");
            }
            await process.WaitForExitAsync();
        }
    }
}