using System.Collections.Generic;
using System.Linq;

namespace System.Population.Random
{
    internal sealed class PopulationServiceSelector
    {
        public static PopulationServiceSelector Instance { get; } = new();
        private PopulationServiceSelector() { }
        public List<IRandomPopulationService> Services { get; } = new();
        public IRandomPopulationService? GetRightService(Type type)
            => Services.OrderByDescending(x => x.Priority).FirstOrDefault(x => x.IsValid(type));
        internal bool TryAdd(IRandomPopulationService service)
        {
            if (!Services.Any(x => x.GetType() == service.GetType()))
            {
                Services.Add(service);
                return true;
            }
            return false;
        }
    }
}