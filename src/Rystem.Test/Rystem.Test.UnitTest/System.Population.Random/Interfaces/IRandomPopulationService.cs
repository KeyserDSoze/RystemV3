using System;

namespace System.Population.Random
{
    /// <summary>
    /// Population service to allow the random creation of values.
    /// </summary>
    public interface IRandomPopulationService
    {
        int Priority { get; }
        bool IsValid(Type type);
        dynamic GetValue(RandomPopulationOptions options);
    }
}