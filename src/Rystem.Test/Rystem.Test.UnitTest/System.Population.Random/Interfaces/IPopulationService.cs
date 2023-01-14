using System;

namespace System.Population.Random
{
    /// <summary>
    /// Population service to allow the in memory population.
    /// </summary>
    public interface IPopulationService
    {
        IInstanceCreator InstanceCreator { get; }
        dynamic? Construct(PopulationSettings settings, Type type, int numberOfEntities, string treeName, string name);
    }
}