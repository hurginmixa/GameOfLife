using System.Collections.Generic;
using System.Linq;

namespace GameOfLifeAppl
{
    internal class ParamsCollection
    {
        private readonly IReadOnlyDictionary<string, string> _params;

        public ParamsCollection(IReadOnlyDictionary<string, string> dataParams)
        {
            _params = dataParams;
        }

        public int Count => _params.Count;

        public string NeighborhoodStrategyName => _params.TryGetValue("Neighborhood", out string value) ? value : "Moore";

        public string RulesStrategyName => _params.TryGetValue("Rules", out string value) ? value : "Life";

        public int StepCount => _params.TryGetValue("Steps", out string value) ? int.Parse(value) : 1;

        public int Generations => _params.TryGetValue("Generations", out string value) ? int.Parse(value) : 1;

        public int[] LifeSurvivals => GetList("Survivals", new[] {2, 3});
        
        public int[] LifeNewBirth => GetList("NewBirth", new[] {3});

        private int[] GetList(string paramName, int[] defaultValues)
        {
            if (!_params.TryGetValue(paramName, out string data))
            {
                return defaultValues;
            }

            if (string.IsNullOrWhiteSpace(data))
            {
                return new[] {0};
            }

            return data.Split(',').Select(int.Parse).ToArray();
        }
    }
}