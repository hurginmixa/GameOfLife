using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLifeAppl.Strategies
{
    internal abstract class ANextStepStrategy : INextStepStrategy
    {
        public bool IsNewCellPolicy(ICellIndex cellIndex, PlayData playData)
        {
            return !cellIndex.IsLifeCell && CheckNeighborsCountForNewCell(cellIndex, playData);
        }

        protected abstract bool CheckNeighborsCountForNewCell(ICellIndex cellIndex, PlayData playData);

        public bool IsDyingCellPolicy(ICellIndex cellIndex, PlayData playData)
        {
            return cellIndex.IsLifeCell && !CheckNeighborsCountForSurvivals(cellIndex, playData);
        }

        protected abstract bool CheckNeighborsCountForSurvivals(ICellIndex cellIndex, PlayData playData);

        public static INextStepStrategy GetStrategy(string strategyName, IReadOnlyDictionary<string, string> @params)
        {
            INextStepStrategy strategy;
            switch (strategyName)
            {
                case "Life":
                {
                    var survivalsNeighborsCounts = new[] {2, 3};
                    
                    var newNeighborsCounts = new[] {3};

                    if (@params.TryGetValue("Survivals", out string data))
                    {
                        survivalsNeighborsCounts = data.Split(',').Select(int.Parse).ToArray();
                    }

                    if (@params.TryGetValue("NewBirth", out data))
                    {
                        newNeighborsCounts = data.Split(',').Select(int.Parse).ToArray();
                    }

                    strategy = new ParametersNextStepStrategy(survivalsNeighborsCounts: survivalsNeighborsCounts, newNeighborsCounts: newNeighborsCounts);
                    break;
                }

                case "HighLife":
                    strategy = new ParametersNextStepStrategy(survivalsNeighborsCounts: new[] {2, 3}, newNeighborsCounts: new[] {3, 6});
                    break;

                case "Diamoeba":
                    strategy = new ParametersNextStepStrategy(survivalsNeighborsCounts: new[] {5, 6, 7, 8}, newNeighborsCounts: new[] {3, 5, 6, 7, 8});
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(strategyName));
            }

            return strategy;
        }

        protected static int GetLifeNeighborsCount(ICellIndex cellIndex, PlayData playData)
        {
            int neighborsCount = 0;

            foreach (var tmpCellIndex in CellIndexEnum(cellIndex, playData))
            {
                if (tmpCellIndex.IsLifeCell)
                {
                    neighborsCount++;
                }
            }

            return neighborsCount;
        }

        private static IEnumerable<ICellIndex> CellIndexEnum(ICellIndex cellIndex, PlayData playData)
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

                    if (playData.TryMakeCellIndex(tmpCol, tmpRow, out ICellIndex tmpCellIndex))
                    {
                        yield return tmpCellIndex;
                    }
                }
            }
        }
    }
}