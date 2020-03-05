using System.Collections.Generic;

namespace GameOfLifeAppl.Strategies
{
    internal class VonNeumannNeighborhoodStrategy : ANeighborhoodStrategy
    {
        public VonNeumannNeighborhoodStrategy(PlayData playData) : base(playData)
        {
        }

        protected override IEnumerable<ICellIndex> NeighborCellIndexEnum(ICellIndex cellIndex)
        {
            if (PlayData.Area.TryMakeCellIndex(cellIndex.Col + 1, cellIndex.Row, out ICellIndex tmpCellIndex))
            {
                yield return tmpCellIndex;
            }

            if (PlayData.Area.TryMakeCellIndex(cellIndex.Col - 1, cellIndex.Row, out tmpCellIndex))
            {
                yield return tmpCellIndex;
            }

            if (PlayData.Area.TryMakeCellIndex(cellIndex.Col, cellIndex.Row + 1, out tmpCellIndex))
            {
                yield return tmpCellIndex;
            }

            if (PlayData.Area.TryMakeCellIndex(cellIndex.Col, cellIndex.Row - 1, out tmpCellIndex))
            {
                yield return tmpCellIndex;
            }
        }
    }
}