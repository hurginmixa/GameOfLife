namespace GameOfLifeAppl.Strategies
{
    internal class HighLifeProcessingCellStrategy : ACellProcessingStrategy
    {
        protected override bool CheckNeighborsCountForNewCell(ICellIndex cellIndex, PlayData playData)
        {
            var neighborsCount = ACellProcessingStrategy.GetLifeNeighborsCount(cellIndex, playData);
            return neighborsCount == 3 || neighborsCount == 6;
        }

        protected override bool CheckNeighborsCountForDying(ICellIndex cellIndex, PlayData playData)
        {
            var neighborsCount = ACellProcessingStrategy.GetLifeNeighborsCount(cellIndex, playData);
            return neighborsCount != 2 && neighborsCount != 3;
        }
    }
}