using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace System.Population.Random
{
    internal sealed class PopulationBuilder<T> : IPopulationBuilder<T>
    {
        private readonly IPopulationStrategy<T> _populationStrategy;
        private readonly int _numberOfElements;
        private readonly int _numberOfElementsWhenEnumerableIsFound;
        private readonly PopulationSettings<T> _settings = new();
        public PopulationBuilder(IPopulationStrategy<T> populationStrategy, int numberOfElements, int numberOfElementsWhenEnumerableIsFound)
        {
            _populationStrategy = populationStrategy;
            _numberOfElements = numberOfElements;
            _numberOfElementsWhenEnumerableIsFound = numberOfElementsWhenEnumerableIsFound;
        }
        private const string LinqFirst = "First().";

        private static string GetNameOfProperty<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
            => string.Join(".", navigationPropertyPath.ToString().Split('.').Skip(1)).Replace(LinqFirst, string.Empty);
        public IPopulationBuilder<T> WithPattern<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, params string[] regex)
        {
            var nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _settings.RegexForValueCreation;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = regex;
            else
                dictionary.Add(nameOfProperty, regex);
            return this;
        }
        public IPopulationBuilder<T> WithSpecificNumberOfElements<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, int numberOfElements)
        {
            var nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _settings.NumberOfElements;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = numberOfElements;
            else
                dictionary.Add(nameOfProperty, numberOfElements);
            return this;
        }
        public IPopulationBuilder<T> WithValue<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Func<TProperty> creator)
        {
            var nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _settings.DelegatedMethodForValueCreation;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = () => creator.Invoke()!;
            else
                dictionary.Add(nameOfProperty, () => creator.Invoke()!);
            return this;
        }
        public IPopulationBuilder<T> WithValue<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Func<IServiceProvider, Task<TProperty>> valueRetriever)
        {
            var nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _settings.DelegatedMethodForValueRetrieving;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = async (x) => (await valueRetriever.Invoke(x).NoContext())!;
            else
                dictionary.Add(nameOfProperty, async (x) => (await valueRetriever.Invoke(x).NoContext())!);
            return this;
        }
        public IPopulationBuilder<T> WithRandomValue<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath,
            Func<IServiceProvider, Task<IEnumerable<TProperty>>> valuesRetriever)
        {
            var nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _settings.DelegatedMethodWithRandomForValueRetrieving;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = async (x) => (await valuesRetriever.Invoke(x).NoContext()!).Select(x => (object)x!)!;
            else
                dictionary.Add(nameOfProperty, async (x) => (await valuesRetriever.Invoke(x).NoContext()!).Select(x => (object)x!)!);
            return this;
        }
        public IPopulationBuilder<T> WithRandomValue<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> navigationPropertyPath,
           Func<IServiceProvider, Task<IEnumerable<TProperty>>> valuesRetriever)
        {
            var nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _settings.DelegatedMethodWithRandomForValueRetrieving;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = async (x) => (await valuesRetriever.Invoke(x).NoContext()!).Select(x => (object)x!)!;
            else
                dictionary.Add(nameOfProperty, async (x) => (await valuesRetriever.Invoke(x).NoContext()!).Select(x => (object)x!)!);
            return this;
        }
        public IPopulationBuilder<T> WithAutoIncrement<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, TProperty start)
        {
            var nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _settings.AutoIncrementations;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = start!;
            else
                dictionary.Add(nameOfProperty, start!);
            return this;
        }
        public IPopulationBuilder<T> WithImplementation<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Type implementationType)
        {
            var nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _settings.ImplementationForValueCreation;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = implementationType;
            else
                dictionary.Add(nameOfProperty, implementationType);
            return this;
        }
        public IPopulationBuilder<T> WithImplementation<TProperty, TEntity>(Expression<Func<T, TProperty>> navigationPropertyPath)
            => WithImplementation(navigationPropertyPath, typeof(TEntity));
        public List<T> Execute() 
            => _populationStrategy.Populate(_settings, _numberOfElements, _numberOfElementsWhenEnumerableIsFound);
    }
}
