// See https://aka.ms/new-console-template for more information
using Rystem.NugetHelper;
using Rystem.NugetHelper.Engine;
using System.Text.RegularExpressions;

namespace Rystem.Nuget
{
    public class Program
    {
        static Regex regexForVersion = new("<Version>[^<]*</Version>");
        static Dictionary<string, string> newVersionOfLibraries = new();
        static VersionType Type = VersionType.Patch;
        static Regex PackageReference = new("<PackageReference[^>]*>");
        static Regex Include = new("Include=");
        static Regex VersionRegex = new(@"Version=\""[^\""]*\""");
        public static async Task Main()
        {
            string path = @$"{new Regex(@"\\repos\\").Split(Directory.GetCurrentDirectory()).First()}\repos";
            List<string> projectNames = new() { "RepositoryFramework", "RystemV3", "Rystem.Concurrency", "Rystem.BackgroundJob", "Rystem.Queue" };
            var rystemDirectories = new DirectoryInfo(path).GetDirectories().Where(x => projectNames.Contains(x.Name)).ToList();

            Update? currentUpdateTree = UpdateConfiguration.UpdateTree;
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
                currentUpdateTree = currentUpdateTree.Son;
                Console.WriteLine($"Current major version is {context.Version.V}");
            }
        }
        static async Task ReadInDeepAsync(DirectoryInfo directoryInfo, LibraryContext context, Update update)
        {
            bool fileFound = false;
            foreach (var file in directoryInfo!.GetFiles())
            {
                if (file.Name.EndsWith(".csproj") && update.Libraries.Any(x => $"{x.LibraryName}.csproj" == file.Name))
                {
                    var library = update.Libraries.First(x => $"{x.LibraryName}.csproj" == file.Name);
                    if (!newVersionOfLibraries.ContainsKey(library.LibraryName))
                    {
                        using var streamReader = new StreamReader(file.OpenRead());
                        string content = await streamReader.ReadToEndAsync();
                        if (regexForVersion.IsMatch(content))
                        {
                            var currentVersion = regexForVersion.Match(content).Value;
                            var version = new NugetHelper.Version(regexForVersion.Match(content).Value.Split('>').Skip(1).First().Split('<').First());
                            version.NextVersion(Type);
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
    }
}