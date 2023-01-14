namespace System.Population.Random
{
    public sealed class PopulationSettings<T> : PopulationSettings
    {
        public static PopulationSettings<T> Default { get; } = new();
    }
}
