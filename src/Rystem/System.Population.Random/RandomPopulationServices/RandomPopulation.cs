﻿using System.Collections.Generic;

namespace System.Population.Random
{
    internal sealed class RandomPopulation<T> : IPopulation<T>
    {
        private readonly IPopulationStrategy<T> _strategy;
        public RandomPopulation(IPopulationStrategy<T> strategy)
        {
            _strategy = strategy;
        }
        public IPopulationBuilder<T> Setup()
            => new PopulationBuilder<T>(_strategy);
        public List<T> Populate(int numberOfElements = 100, int numberOfElementsWhenEnumerableIsFound = 10)
            => _strategy.Populate(PopulationSettings<T>.Default, numberOfElements, numberOfElementsWhenEnumerableIsFound);
    }
}
