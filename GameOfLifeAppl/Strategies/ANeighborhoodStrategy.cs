using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLifeAppl.Strategies
{
    internal abstract class ANeighborhoodStrategy : INeighborhoodStrategy
    {
        private readonly PlayData _playData;

        protected PlayData PlayData => _playData;

        protected ANeighborhoodStrategy(PlayData playData)
        {
            _playData = playData;
        }

        public int GetLifeNeighborhoodCount(ICellIndex cellIndex)
        {
            return NeighborCellIndexEnum(cellIndex).Count(c => c.IsLifeCell);
        }

        protected abstract IEnumerable<ICellIndex> NeighborCellIndexEnum(ICellIndex cellIndex);
    }
}