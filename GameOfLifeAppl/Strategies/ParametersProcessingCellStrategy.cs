using System.Collections.Generic;
using System.Linq;

namespace GameOfLifeAppl.Strategies
{
    internal class ParametersProcessingCellStrategy : ACellProcessingStrategy
    {
        private readonly int[] _survivalsNeighborsCount;
        private readonly int[] _newNeighborsCount;

        public ParametersProcessingCellStrategy(IEnumerable<int> survivalsNeighborsCounts, IEnumerable<int> newNeighborsCounts)
        {
            _survivalsNeighborsCount = survivalsNeighborsCounts.ToArray();
            _newNeighborsCount = newNeighborsCounts.ToArray();
        }

        protected override bool CheckNeighborsCountForNewCell(ICellIndex cellIndex, PlayData playData)
        {
            var neighborsCount = ACellProcessingStrategy.GetLifeNeighborsCount(cellIndex, playData);
            return _newNeighborsCount.Contains(neighborsCount);
        }

        protected override bool CheckNeighborsCountForDying(ICellIndex cellIndex, PlayData playData)
        {
            var neighborsCount = ACellProcessingStrategy.GetLifeNeighborsCount(cellIndex, playData);
            var contains = !_survivalsNeighborsCount.Contains(neighborsCount);
            return contains;
        }
    }
}