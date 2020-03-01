using System.Collections.Generic;
using System.Linq;

namespace GameOfLifeAppl.Strategies
{
    internal class ParametersNextStepStrategy : ANextStepStrategy
    {
        private readonly int[] _survivalsNeighborsCount;
        private readonly int[] _newNeighborsCount;

        public ParametersNextStepStrategy(IEnumerable<int> survivalsNeighborsCounts, IEnumerable<int> newNeighborsCounts)
        {
            _survivalsNeighborsCount = survivalsNeighborsCounts.ToArray();
            _newNeighborsCount = newNeighborsCounts.ToArray();
        }

        protected override bool CheckNeighborsCountForNewCell(ICellIndex cellIndex, PlayData playData)
        {
            var neighborsCount = ANextStepStrategy.GetLifeNeighborsCount(cellIndex, playData);
            return _newNeighborsCount.Contains(neighborsCount);
        }

        protected override bool CheckNeighborsCountForSurvivals(ICellIndex cellIndex, PlayData playData)
        {
            var neighborsCount = ANextStepStrategy.GetLifeNeighborsCount(cellIndex, playData);
            return _survivalsNeighborsCount.Contains(neighborsCount);
        }
    }
}