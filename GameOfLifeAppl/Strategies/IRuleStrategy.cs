namespace GameOfLifeAppl.Strategies
{
    internal interface IRuleStrategy
    {
        bool IsDyingCellPolicy(ICellIndex cellIndex, int neighborhoodCount);
        
        bool IsNewCellPolicy(ICellIndex cellIndex, int neighborhoodCount);
    }
}