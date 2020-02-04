namespace GameOfLifeAppl
{
    internal interface ICellIndex
    {
        int Col { get; }

        int Row { get; }

        ref char Char { get; }
    }
}