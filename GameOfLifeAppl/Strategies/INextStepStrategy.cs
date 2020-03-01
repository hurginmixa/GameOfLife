namespace GameOfLifeAppl.Strategies
{
    internal interface INextStepStrategy
    {
        bool IsNewCellPolicy(ICellIndex cellIndex, PlayData playData);
            
        bool IsDyingCellPolicy(ICellIndex cellIndex, PlayData playData);
    }
}