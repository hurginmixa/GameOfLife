using System.Collections.Generic;
using System.Linq;

namespace GameOfLifeAppl.Strategies
{
    class MooreNeighborhoodStrategy : ANeighborhoodStrategy
    {
        public MooreNeighborhoodStrategy(PlayData playData) : base(playData)
        {
        }

        protected override IEnumerable<ICellIndex> NeighborCellIndexEnum(ICellIndex cellIndex)
        {
            for (int deltaCol = -1; deltaCol <= 1; deltaCol++)
            {
                for (int deltaRow = -1; deltaRow <= 1; deltaRow++)
                {
                    if (deltaCol == 0 && deltaRow == 0)
                    {
                        continue;
                    }

                    int tmpCol = cellIndex.Col + deltaCol;
                    int tmpRow = cellIndex.Row + deltaRow;

                    if (PlayData.Area.TryMakeCellIndex(tmpCol, tmpRow, out ICellIndex tmpCellIndex))
                    {
                        yield return tmpCellIndex;
                    }
                }
            }
        }
    }
}