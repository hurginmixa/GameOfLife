namespace GameOfLifeAppl.Strategies
{
    internal class RegularProcessingCellStrategy : ACellProcessingStrategy
    {
        protected override bool CheckNeighborsCountForNewCell(ICellIndex cellIndex, PlayData playData)
        {
            var neighborsCount = ACellProcessingStrategy.GetLifeNeighborsCount(cellIndex, playData);
            return neighborsCount == 3;
        }

        protected override bool CheckNeighborsCountForDying(ICellIndex cellIndex, PlayData playData)
        {
            var neighborsCount = ACellProcessingStrategy.GetLifeNeighborsCount(cellIndex, playData);
            return neighborsCount != 2 && neighborsCount != 3;
        }
    }
}