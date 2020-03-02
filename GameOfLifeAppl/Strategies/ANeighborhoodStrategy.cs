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
            return CellIndexEnum(cellIndex).Count(c => c.IsLifeCell);
        }

        public static INeighborhoodStrategy GetStrategy(PlayData playData)
        {
            string strategyName = playData.NeighborhoodStrategyName();
            switch (strategyName)
            {
                case "Moore":
                    return new MooreNeighborhoodStrategy(playData);

                case "vonNeumann":
                    return new VonNeumannNeighborhoodStrategy(playData);

                default:
                    throw new ArgumentOutOfRangeException(nameof(strategyName));
            }
        }

        protected abstract IEnumerable<ICellIndex> CellIndexEnum(ICellIndex cellIndex);
    }

    internal class VonNeumannNeighborhoodStrategy : ANeighborhoodStrategy
    {
        public VonNeumannNeighborhoodStrategy(PlayData playData) : base(playData)
        {
        }

        protected override IEnumerable<ICellIndex> CellIndexEnum(ICellIndex cellIndex)
        {
            if (PlayData.TryMakeCellIndex(cellIndex.Col + 1, cellIndex.Row, out ICellIndex tmpCellIndex))
            {
                yield return tmpCellIndex;
            }

            if (PlayData.TryMakeCellIndex(cellIndex.Col - 1, cellIndex.Row, out tmpCellIndex))
            {
                yield return tmpCellIndex;
            }

            if (PlayData.TryMakeCellIndex(cellIndex.Col, cellIndex.Row + 1, out tmpCellIndex))
            {
                yield return tmpCellIndex;
            }

            if (PlayData.TryMakeCellIndex(cellIndex.Col, cellIndex.Row - 1, out tmpCellIndex))
            {
                yield return tmpCellIndex;
            }
        }
    }
}