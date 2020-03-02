using System.Collections.Generic;
using System.Linq;

namespace GameOfLifeAppl.Strategies
{
    internal class ParametersRuleStrategy : ARuleStrategy
    {
        private readonly int[] _survivalsNeighborsCount;
        private readonly int[] _newNeighborsCount;

        public ParametersRuleStrategy(IEnumerable<int> survivalsNeighborsCounts, IEnumerable<int> newNeighborsCounts)
        {
            _survivalsNeighborsCount = survivalsNeighborsCounts.ToArray();
            _newNeighborsCount = newNeighborsCounts.ToArray();
        }

        protected override bool CheckNeighborsCountForNewCell(ICellIndex cellIndex, int neighborhoodCount)
        {
            return _newNeighborsCount.Contains(neighborhoodCount);
        }

        protected override bool CheckNeighborsCountForSurvivals(ICellIndex cellIndex, int neighborhoodCount)
        {
            return _survivalsNeighborsCount.Contains(neighborhoodCount);
        }
    }
}