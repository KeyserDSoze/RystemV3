using System.Collections.Generic;

namespace System.Population.Random
{
    public interface IPopulation<T>
    {
        IPopulationBuilder<T> Setup();
        List<T> Populate(int numberOfElements = 100, int numberOfElementsWhenEnumerableIsFound = 10);
    }
}
