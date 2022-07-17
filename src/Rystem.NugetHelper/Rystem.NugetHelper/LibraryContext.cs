namespace Rystem.NugetHelper
{
    internal enum VersionType
    {
        Major,
        Minor,
        Patch
    }
    internal record Version
    {
        public Version(string v)
        {
            V = v;
        }
        public int Major => int.Parse(V.Split('.').First());
        public int Minor => int.Parse(V.Split('.').Skip(1).First());
        public int Patch => int.Parse(V.Split('.').Skip(2).First());

        public string V { get; private set; }

        public bool IsGreater(Version context)
        {
            if (context.Major > Major)
                return true;
            else if (context.Major == Major)
            {
                if (context.Minor > Minor)
                    return true;
                else if (context.Minor == Minor)
                    return context.Patch > Patch;
            }
            return false;
        }
        public void NextVersion(VersionType type)
        {
            switch (type)
            {
                case VersionType.Major:
                    V = $"{Major + 1}.0.0";
                    break;
                case VersionType.Minor:
                    V = $"{Major}.{Minor + 1}.0";
                    break;
                case VersionType.Patch:
                    V = $"{Major}.{Minor}.{Patch + 1}";
                    break;
            }
        }
    }
    internal record LibraryContext
    {
        public string LibraryName { get; set; }
        public LibraryContext(string version = "0.0.0")
        {
            Version = new(version);
        }
        public Version Version { get; internal set; }
    }
}
