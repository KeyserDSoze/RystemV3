using System.Collections.Generic;

namespace System.Population.Random
{
    internal sealed class RandomPopulation<T> : IPopulation<T>
    {
        private readonly IPopulationStrategy<T> _strategy;
        public RandomPopulation(IPopulationStrategy<T> strategy)
        {
            _strategy = strategy;
        }
        public IPopulationBuilder<T> Populate(int numberOfElements = 100, int numberOfElementsWhenEnumerableIsFound = 10)
            => new PopulationBuilder<T>(_strategy, numberOfElements, numberOfElementsWhenEnumerableIsFound);
        public List<T> PopulateWithDefault(int numberOfElements = 100, int numberOfElementsWhenEnumerableIsFound = 10)
            => _strategy.Populate(PopulationSettings<T>.Default, numberOfElements, numberOfElementsWhenEnumerableIsFound);
    }
}
