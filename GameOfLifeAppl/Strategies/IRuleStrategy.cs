namespace GameOfLifeAppl.Strategies
{
    internal interface IRuleStrategy
    {
        bool IsDyingCellPolicy(ICellIndex cellIndex, int neighborhoodCount, int maxGenerations);
        
        bool IsNewCellPolicy(ICellIndex cellIndex, int neighborhoodCount);
    }
}