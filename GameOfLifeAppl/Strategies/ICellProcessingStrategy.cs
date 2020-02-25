namespace GameOfLifeAppl.Strategies
{
    internal interface ICellProcessingStrategy
    {
        bool IsNewCellPolicy(ICellIndex cellIndex, PlayData playData);
            
        bool IsDyingCellPolicy(ICellIndex cellIndex, PlayData playData);
    }
}