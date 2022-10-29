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
            .AddProject("Rystem.Concurrency", "Rystem.RepositoryFramework.Abstractions")
            .CreateSon()
            .AddProject("Rystem.BackgroundJob",
            "Rystem.RepositoryFramework.Api.Client", "Rystem.RepositoryFramework.Api.Server",
            "Rystem.RepositoryFramework.Infrastructure.InMemory", "Rystem.RepositoryFramework.MigrationTools",
            "Rystem.RepositoryFramework.Cache", "Rystem.RepositoryFramework.Infrastructure.Azure.Cosmos.Sql",
            "Rystem.RepositoryFramework.Infrastructure.Azure.Storage.Blob", "Rystem.RepositoryFramework.Infrastructure.Azure.Storage.Table",
            "Rystem.RepositoryFramework.Infrastructure.Dynamics.Dataverse", "Rystem.RepositoryFramework.Infrastructure.EntityFramework",
            "Rystem.RepositoryFramework.Infrastructure.MsSql")
            .CreateSon()
            .AddProject("Rystem.Queue", "Rystem.RepositoryFramework.Cache.Azure.Storage.Blob");

            OnlyRepositoryTree = new Update()
            .AddProject("Rystem.RepositoryFramework.Abstractions");
            OnlyRepositoryTree
            .CreateSon()
            .AddProject(
            "Rystem.RepositoryFramework.Api.Client", "Rystem.RepositoryFramework.Api.Server",
            "Rystem.RepositoryFramework.Infrastructure.InMemory", "Rystem.RepositoryFramework.MigrationTools",
            "Rystem.RepositoryFramework.Cache", "Rystem.RepositoryFramework.Infrastructure.Azure.Cosmos.Sql",
            "Rystem.RepositoryFramework.Infrastructure.Azure.Storage.Blob", "Rystem.RepositoryFramework.Infrastructure.Azure.Storage.Table",
            "Rystem.RepositoryFramework.Infrastructure.Dynamics.Dataverse", "Rystem.RepositoryFramework.Infrastructure.EntityFramework",
            "Rystem.RepositoryFramework.Infrastructure.MsSql")
            .CreateSon()
            .AddProject("Rystem.RepositoryFramework.Cache.Azure.Storage.Blob");
        }
        public static Update UpdateTree { get; }
        public static Update OnlyRepositoryTree { get; }
    }
}
