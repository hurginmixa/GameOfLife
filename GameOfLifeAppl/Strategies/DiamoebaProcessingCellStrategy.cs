namespace GameOfLifeAppl.Strategies
{
    internal class DiamoebaProcessingCellStrategy : ACellProcessingStrategy
    {
        protected override bool CheckNeighborsCountForNewCell(ICellIndex cellIndex, PlayData playData)
        {
            var neighborsCount = ACellProcessingStrategy.GetLifeNeighborsCount(cellIndex, playData);
            return neighborsCount == 3 || neighborsCount == 5 || neighborsCount == 6 || neighborsCount == 7 || neighborsCount == 8;
        }

        protected override bool CheckNeighborsCountForDying(ICellIndex cellIndex, PlayData playData)
        {
            var neighborsCount = ACellProcessingStrategy.GetLifeNeighborsCount(cellIndex, playData);
            return neighborsCount != 5 && neighborsCount != 6 && neighborsCount != 7 && neighborsCount != 8;
        }
    }
}