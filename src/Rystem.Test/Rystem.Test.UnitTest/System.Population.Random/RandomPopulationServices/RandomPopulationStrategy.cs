﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Population.Random
{
    internal sealed class RandomPopulationStrategy<T> : IPopulationStrategy<T>
    {
        private readonly IPopulationService _populationService;
        private readonly IInstanceCreator _instanceCreator;
        private readonly PopulationServiceSettings _settings;

        public RandomPopulationStrategy(IPopulationService populationService,
            IInstanceCreator instanceCreator,
            PopulationServiceSettings defaultsettings,
            PopulationServiceSettings<T>? settings = null)
        {
            _populationService = populationService;
            _instanceCreator = instanceCreator;
            _settings = settings ?? defaultsettings;
        }
        public List<T> Populate(int numberOfElements = 100, int numberOfElementsWhenEnumerableIsFound = 10)
        {
            List<T> items = new();
            _populationService.Settings = _settings.BehaviorSettings ?? new();
            var properties = typeof(T).GetProperties();
            for (var i = 0; i < numberOfElements; i++)
            {
                var entity = _instanceCreator!.CreateInstance(new RandomPopulationOptions(typeof(T),
                    _populationService!, numberOfElementsWhenEnumerableIsFound, string.Empty));
                foreach (var property in properties.Where(x => x.CanWrite))
                {
                    if (property.PropertyType == typeof(Range) ||
                            GetDefault(property.PropertyType) == (property.GetValue(entity) as dynamic))
                    {
                        var value = _populationService!.Construct(property.PropertyType,
                            numberOfElementsWhenEnumerableIsFound, string.Empty,
                            property.Name);
                        if (property.PropertyType.GetInterface(nameof(IList)) != null && value is IEnumerable enumerable)
                        {
                            var list = (Activator.CreateInstance(property.PropertyType) as IList)!;
                            foreach (var singleItem in enumerable)
                                list.Add(singleItem);
                            value = list;
                        }
                        property.SetValue(entity, value);
                    }
                }
                var item = (T)entity!;
                items.Add(item);
            }
            return items;
        }
        private static dynamic GetDefault(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type)!;
            return null!;
        }
    }
}
