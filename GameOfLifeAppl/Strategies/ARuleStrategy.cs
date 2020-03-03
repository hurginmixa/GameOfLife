using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLifeAppl.Strategies
{
    internal abstract class ARuleStrategy : IRuleStrategy
    {
        public bool IsNewCellPolicy(ICellIndex cellIndex, int neighborhoodCount)
        {
            return !cellIndex.IsLifeCell && CheckNeighborsCountForNewCell(cellIndex, neighborhoodCount);
        }

        protected abstract bool CheckNeighborsCountForNewCell(ICellIndex cellIndex, int neighborhoodCount);

        public bool IsDyingCellPolicy(ICellIndex cellIndex, int neighborhoodCount)
        {
            return cellIndex.IsLifeCell && !CheckNeighborsCountForSurvivals(cellIndex, neighborhoodCount);
        }

        protected abstract bool CheckNeighborsCountForSurvivals(ICellIndex cellIndex, int neighborhoodCount);
    }
}