namespace Rystem.NugetHelper.Engine
{
    internal class Update
    {
        public List<LibraryContext> Libraries { get; set; } = new();
        public Update? Son { get; set; }
        public Update CreateSon()
            => Son = new Update();
        public Update AddProject(params string[] projectNames)
        {
            Libraries.AddRange(projectNames.Select(x => new LibraryContext { LibraryName = x }));
            return this;
        }
    }
    internal static class UpdateConfiguration
    {
        static UpdateConfiguration()
        {
            UpdateTree = new Update()
            .AddProject("Rystem");
            UpdateTree
            .CreateSon()
            .AddProject("Rystem.DependencyInjectionExtensions", "RepositoryFramework.Abstractions")
            .CreateSon()
            .AddProject("Rystem.Concurrency",
            "RepositoryFramework.Api.Client", "RepositoryFramework.Api.Server",
            "RepositoryFramework.Infrastructure.InMemory", "RepositoryFramework.MigrationTools",
            "RepositoryFramework.Cache", "RepositoryFramework.Infrastructure.Azure.Cosmos.Sql",
            "RepositoryFramework.Infrastructure.Azure.Storage.Blob", "RepositoryFramework.Infrastructure.Azure.Storage.Table")
            .CreateSon()
            .AddProject("Rystem.BackgroundJob", "RepositoryFramework.Cache.Azure.Storage.Blob")
            .CreateSon()
            .AddProject("Rystem.Queue");
        }
        public static Update UpdateTree { get; }
    }
}
