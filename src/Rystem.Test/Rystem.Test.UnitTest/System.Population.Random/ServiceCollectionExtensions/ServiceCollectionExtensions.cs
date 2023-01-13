using System;
using System.Population.Random;

namespace Microsoft.Extensions.DependencyInjection.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Override the population default service.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddPopulationService(
          this IServiceCollection services)
        {
            services.AddSingleton<IPopulationService, PopulationService>();
            services.AddSingleton<IInstanceCreator, InstanceCreator>();
            services.AddSingleton<IRegexService, RegexService>();
            PopulationServiceSelector.Instance.TryAdd(new AbstractPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new ArrayPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new BoolPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new BytePopulationService());
            PopulationServiceSelector.Instance.TryAdd(new CharPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new ObjectPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new DictionaryPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new EnumerablePopulationService());
            PopulationServiceSelector.Instance.TryAdd(new GuidPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new NumberPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new RangePopulationService());
            PopulationServiceSelector.Instance.TryAdd(new StringPopulationService());
            PopulationServiceSelector.Instance.TryAdd(new TimePopulationService());
            services.AddSingleton(PopulationServiceSelector.Instance);
            services.TryAddSingleton(typeof(IPopulationStrategy<>), typeof(RandomPopulationStrategy<>));
            services.TryAddSingleton(typeof(IPopulation<>), typeof(RandomPopulation<>));
            _ = services.AddSingleton(
                new PopulationServiceSettings
                {
                    BehaviorSettings = new(),
                });
            return services;
        }
        /// <summary>
        /// Override the population strategy default service.
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <typeparam name="TService">your IPopulationService</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddPopulationService<T, TService>(
          this IServiceCollection services)
          where TService : class, IPopulation<T>
          => services.AddSingleton<IPopulation<T>, TService>();
        /// <summary>
        /// Override the population default service.
        /// </summary>
        /// <typeparam name="TService">your IPopulationService</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddPopulationService<TService>(
          this IServiceCollection services)
          where TService : class, IPopulationService
          => services.AddSingleton<IPopulationService, TService>();
        /// <summary>
        /// Add specific population settings.
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddPopulationSettings<T>(
          this IServiceCollection services,
          Action<PopulationServiceSettings<T>>? settings)
        {
            var defaultSettings = new PopulationServiceSettings<T>
            {
                BehaviorSettings = new(),
            };
            settings?.Invoke(defaultSettings);
            return services.AddSingleton(defaultSettings);
        }
        /// <summary>
        /// Override the population strategy default service.
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <typeparam name="TService">your IPopulationService</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddPopulationStrategyService<T, TService>(
          this IServiceCollection services)
          where TService : class, IPopulationStrategy<T>
          => services.AddSingleton<IPopulationStrategy<T>, TService>();
        /// <summary>
        /// Override the default instance creator for you population service.
        /// </summary>
        /// <typeparam name="T">your IInstanceCreator</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddInstanceCreatorServiceForPopulation<T>(
            this IServiceCollection services)
            where T : class, IInstanceCreator
            => services.AddSingleton<IInstanceCreator, T>();
        /// <summary>
        /// Override the default regular expression service for you population service.
        /// </summary>
        /// <typeparam name="T">your IRegexService</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRegexService<T>(
            this IServiceCollection services)
            where T : class, IRegexService
            => services.AddSingleton<IRegexService, T>();
        /// <summary>
        /// Add a random population service to your population service, you can use Priority property to override default behavior.
        /// For example a service for string random generation already exists with Priority 1,
        /// you may create another string random service with Priority = 2 or greater of 1.
        /// In IsValid method you have to check if type is a string to complete the override.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRandomPopulationService(this IServiceCollection services,
            IRandomPopulationService service)
        {
            PopulationServiceSelector.Instance.TryAdd(service);
            services.AddSingleton(PopulationServiceSelector.Instance);
            return services;
        }
    }
}
