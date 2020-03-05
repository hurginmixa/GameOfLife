using System;
using System.Collections.Generic;
using System.IO;

namespace GameOfLifeAppl
{
    internal class PlayArea
    {
        #region private class CellIndex : ICellIndex

        private class CellIndex : ICellIndex
        {
            private const int DyingCellChar = -2;
            private const int NewCellChar = -1;
            private const int EmptyCellChar = -3;

            private readonly PlayArea _playArea;

            public CellIndex(int col, int row, PlayArea playArea)
            {
                _playArea = playArea;
                Col = col;
                Row = row;
            }

            public int Col { get; }

            public int Row { get; }

            public bool IsLifeCell => (Cell != DyingCellChar && Cell != NewCellChar && Cell != EmptyCellChar) || Cell == DyingCellChar;

            public bool IsEmptyCell => Cell == EmptyCellChar;

            public bool IsDyingCell => Cell == DyingCellChar;

            public bool IsNewCell => Cell == NewCellChar;

            public int Generation => Cell;

            public void SetDyingCell()
            {
                Cell = DyingCellChar;
            }

            public void SetNewCell()
            {
                Cell = NewCellChar;
            }

            public void SetEmptyCell()
            {
                Cell = EmptyCellChar;
            }

            public void SetLifeCell()
            {
                Cell = 0;
            }

            public void IncGeneration()
            {
                Cell =+ 1;
            }

            private ref int Cell => ref _playArea[Col, Row];
        }

        #endregion

        private readonly int[,] _area;

        public PlayArea(char[,] area)
        {
            _area = new int[area.GetLength(0), area.GetLength(1)];

            for (int col = 0; col < area.GetLength(0); col++)
            {
                for (int row = 0; row < area.GetLength(1); row++)
                {
                    ICellIndex cellIndex;
                    TryMakeCellIndex(col, row, out cellIndex);

                    switch (area[col, row])
                    {
                        case '.':
                        case '\0':
                            cellIndex.SetEmptyCell();
                            break;

                        case 'X':
                        case '0':
                            cellIndex.SetLifeCell();
                            break;

                        default:
                            throw new Exception($"Invalid cell input {area[col, row]}");
                    }
                }
            }
        }

        public int Cols => _area.GetLength(0);

        public int Rows => _area.GetLength(1);

        private ref int this[int col, int row] => ref _area[col, row];

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

                else if (cellIndex.IsLifeCell)
                {
                    cellIndex.IncGeneration();
                }
            }
        }

        public void WriteArea(string outFile, bool printGenerations)
        {
            using (TextWriter tw = new StreamWriter(outFile))
            {
                tw.WriteLine($"{Cols} {Rows}");
                
                for (int row = 0; row < Rows; row++)
                {
                    for (int col = 0; col < Cols; col++)
                    {
                        TryMakeCellIndex(col, row, out ICellIndex cellIndex);

                        if (printGenerations)
                        {
                            tw.Write(cellIndex.IsLifeCell ? cellIndex.Generation.ToString()[0] : '.');
                            continue;
                        }

                        tw.Write(cellIndex.IsLifeCell ? 'X' : '.');
                    }

                    tw.WriteLine();
                }
            }
        }
    }
}