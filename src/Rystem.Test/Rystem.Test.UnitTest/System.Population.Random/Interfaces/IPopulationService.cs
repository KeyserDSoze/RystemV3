using System;

namespace System.Population.Random
{
    /// <summary>
    /// Population service to allow the in memory population.
    /// </summary>
    public interface IPopulationService
    {
        PopulationSettings Settings { get; set; }
        IInstanceCreator InstanceCreator { get; }
        dynamic? Construct(Type type, int numberOfEntities, string treeName, string name);
    }
}