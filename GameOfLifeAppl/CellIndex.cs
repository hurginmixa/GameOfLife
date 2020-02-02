namespace GameOfLifeAppl
{
    internal class CellIndex
    {
        public CellIndex(int col, int row)
        {
            Col = col;
            Row = row;
        }

        public int Col { get; }

        public int Row { get; }
    }
}