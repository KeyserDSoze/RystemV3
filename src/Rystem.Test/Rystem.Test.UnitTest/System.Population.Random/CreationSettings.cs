using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Population.Random
{
    public sealed class PopulationSettings<T>
    {
        public static PopulationSettings<T> Default { get; } = new();
        public Dictionary<string, string[]> RegexForValueCreation { get; set; } = new();
        public Dictionary<string, Func<dynamic>> DelegatedMethodForValueCreation { get; set; } = new();
        public Dictionary<string, Func<IServiceProvider, Task<dynamic>>> DelegatedMethodForValueRetrieving { get; set; } = new();
        public Dictionary<string, Func<IServiceProvider, Task<IEnumerable<dynamic>>>> DelegatedMethodWithRandomForValueRetrieving { get; set; } = new();
        public Dictionary<string, dynamic> AutoIncrementations { get; set; } = new();
        public Dictionary<string, Type> ImplementationForValueCreation { get; set; } = new();
        public Dictionary<string, int> NumberOfElements { get; set; } = new();
    }
}
