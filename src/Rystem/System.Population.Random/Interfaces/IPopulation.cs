using System.Collections.Generic;

namespace System.Population.Random
{
    public interface IPopulation<T>
    {
        IPopulationBuilder<T> Populate(int numberOfElements = 100, int numberOfElementsWhenEnumerableIsFound = 10);
        List<T> PopulateWithDefault(int numberOfElements = 100, int numberOfElementsWhenEnumerableIsFound = 10);
    }
}
