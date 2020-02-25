using System;

namespace GameOfLifeAppl.Strategies
{
    internal abstract class ACellProcessingStrategy : ICellProcessingStrategy
    {
        public bool IsNewCellPolicy(ICellIndex cellIndex, PlayData playData)
        {
            return !cellIndex.IsLifeCell && CheckNeighborsCountForNewCell(cellIndex, playData);
        }

        protected abstract bool CheckNeighborsCountForNewCell(ICellIndex cellIndex, PlayData playData);

        public bool IsDyingCellPolicy(ICellIndex cellIndex, PlayData playData)
        {
            return cellIndex.IsLifeCell && CheckNeighborsCountForDying(cellIndex, playData);
        }

        protected abstract bool CheckNeighborsCountForDying(ICellIndex cellIndex, PlayData playData);

        public static ICellProcessingStrategy GetStrategy(string paramValue)
        {
            ICellProcessingStrategy strategy;
            switch (paramValue)
            {
                case "Life":
                    strategy = new RegularProcessingCellStrategy();
                    break;

                case "HighLife":
                    strategy = new HighLifeProcessingCellStrategy();
                    break;

                case "Diamoeba":
                    strategy = new DiamoebaProcessingCellStrategy();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(paramValue));
            }

            return strategy;
        }

        public static int GetLifeNeighborsCount(ICellIndex cellIndex, PlayData playData)
        {
            int neighborsCount = 0;

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
                        if (tmpCellIndex.IsLifeCell)
                        {
                            neighborsCount++;
                        }
                    }
                }
            }

            return neighborsCount;
        }
    }
}