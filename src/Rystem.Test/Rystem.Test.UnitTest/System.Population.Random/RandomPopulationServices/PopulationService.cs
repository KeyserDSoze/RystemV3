﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Population.Random
{
    internal class PopulationService : IPopulationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<IRandomPopulationService> _randomPopulationServices;
        private readonly IRegexService _regexService;
        public IInstanceCreator InstanceCreator { get; }
        public PopulationSettings Settings { get; set; } = null!;
        public PopulationService(
            IServiceProvider serviceProvider,
            IEnumerable<IRandomPopulationService> randomPopulationServices,
            IRegexService regexService,
            IInstanceCreator instanceCreator)
        {
            _serviceProvider = serviceProvider;
            _randomPopulationServices = randomPopulationServices;
            _regexService = regexService;
            InstanceCreator = instanceCreator;
        }

        public dynamic? Construct(Type type, int numberOfEntities, string treeName, string name)
        {
            if (!string.IsNullOrWhiteSpace(treeName) && !string.IsNullOrWhiteSpace(name))
                treeName = $"{treeName}.{name}";
            else if (!string.IsNullOrWhiteSpace(name))
                treeName = name;

            int? overridedNumberOfEntities = null;
            var numberOfEntitiesDictionary = Settings.NumberOfElements;
            if (numberOfEntitiesDictionary.ContainsKey(treeName))
                overridedNumberOfEntities = numberOfEntitiesDictionary[treeName];
            numberOfEntities = overridedNumberOfEntities ?? numberOfEntities;

            if (Settings.DelegatedMethodForValueCreation.ContainsKey(treeName))
                return Settings.DelegatedMethodForValueCreation[treeName].Invoke();

            if (Settings.DelegatedMethodForValueRetrieving.ContainsKey(treeName))
                return Settings.DelegatedMethodForValueRetrieving[treeName].Invoke(_serviceProvider).ToResult();

            if (Settings.DelegatedMethodWithRandomForValueRetrieving.ContainsKey(treeName))
            {
                var entities = Settings.DelegatedMethodWithRandomForValueRetrieving[treeName].Invoke(_serviceProvider).ToResult();
                var count = entities.Count() - numberOfEntities;
                var index = System.Random.Shared.Next(0, count);
                return entities.Skip(index).Take(numberOfEntities);
            }

            if (Settings.RegexForValueCreation.ContainsKey(treeName))
                return _regexService.GetRandomValue(type,
                    Settings.RegexForValueCreation[treeName]);

            if (Settings.AutoIncrementations.ContainsKey(treeName))
                return Settings.AutoIncrementations[treeName]++;

            if (Settings.ImplementationForValueCreation.ContainsKey(treeName) && !string.IsNullOrWhiteSpace(name))
                return Construct(Settings.ImplementationForValueCreation[treeName], numberOfEntities,
                    treeName, string.Empty);

            var service = _randomPopulationServices.OrderByDescending(x => x.Priority).FirstOrDefault(x => x.IsValid(type));
            if (service != default)
                return service.GetValue(new RandomPopulationOptions(type, this, numberOfEntities, treeName));
            return default;
        }
    }
}
