namespace GameOfLifeAppl.Strategies
{
    internal interface INeighborhoodStrategy
    {
        int GetLifeNeighborhoodCount(ICellIndex cellIndex);
    }
}