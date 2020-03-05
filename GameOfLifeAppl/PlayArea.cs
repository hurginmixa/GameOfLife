using System;
using System.Collections.Generic;

namespace GameOfLifeAppl
{
    internal class PlayArea
    {
        #region private class CellIndex : ICellIndex

        private class CellIndex : ICellIndex
        {
            private const char LifeCellChar = 'X';
            private const char DyingCellChar = 'x';
            private const char NewCellChar = 'n';
            private const char EmptyCellChar = '.';

            private readonly PlayArea _playArea;

            public CellIndex(int col, int row, PlayArea playArea)
            {
                _playArea = playArea;
                Col = col;
                Row = row;
            }

            public int Col { get; }

            public int Row { get; }

            public bool IsLifeCell => Char == LifeCellChar || Char == DyingCellChar;

            public bool IsDyingCell => Char == DyingCellChar;

            public bool IsNewCell => Char == NewCellChar;

            public void SetDyingCell()
            {
                Char = DyingCellChar;
            }

            public void SetNewCell()
            {
                Char = NewCellChar;
            }

            public void SetEmptyCell()
            {
                Char = EmptyCellChar;
            }

            public void SetLifeCell()
            {
                Char = LifeCellChar;
            }

            private ref char Char => ref _playArea[Col, Row];
        }

        #endregion

        private readonly char[,] _area;

        public PlayArea(char[,] area)
        {
            _area = area;
        }

        public int Cols => _area.GetLength(0);

        public int Rows => _area.GetLength(1);

        public ref char this[int col, int row] => ref _area[col, row];

        public bool TryMakeCellIndex(int col, int row, out ICellIndex cellIndex)
        {
            cellIndex = col >= 0 && col < Cols && row >= 0 && row < Rows 
                ? new CellIndex(col, row, this) 
                : null;

            return cellIndex != null;
        }

        public IEnumerable<ICellIndex> GetCellIndexes() => GetCellIndexes(filter: c => true);
        
        public IEnumerable<ICellIndex> GetCellIndexes(Func<ICellIndex, bool> filter)
        {
            for (int col = 0; col < Cols; col++)
            {
                for (int row = 0; row < Rows; row++)
                {
                    if (!TryMakeCellIndex(col, row, out var cellIndex))
                    {
                        continue;
                    }

                    if (filter(cellIndex))
                    {
                        yield return cellIndex;
                    }
                }
            }
        }

        public void UpdateCells()
        {
            foreach (var cellIndex in GetCellIndexes())
            {
                if (cellIndex.IsDyingCell)
                {
                    cellIndex.SetEmptyCell();
                }

                else if (cellIndex.IsNewCell)
                {
                    cellIndex.SetLifeCell();
                }
            }
        }
    }
}