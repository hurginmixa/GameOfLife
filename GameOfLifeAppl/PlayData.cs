using System;
using System.Collections.Generic;
using System.IO;

namespace GameOfLifeAppl
{
    internal class PlayData
    {
        #region private class CellIndex : ICellIndex

        private class CellIndex : ICellIndex
        {
            private readonly PlayData _playData;

            public CellIndex(int col, int row, PlayData playData)
            {
                _playData = playData;
                Col = col;
                Row = row;
            }

            public int Col { get; }

            public int Row { get; }

            public ref char Char => ref _playData._area[Col, Row];
        }

        #endregion

        public interface INewCellStrategy
        {
            bool IsNewCell(ICellIndex cellIndex, PlayData playData);
        }

        public class RegularNewCellStrategy : INewCellStrategy
        {
            public bool IsNewCell(ICellIndex cellIndex, PlayData playData)
            {
                var neighborsCount = playData.GetLifeNeighborsCount(cellIndex);
                return neighborsCount == 3;
            }
        }

        public class HighLifeNewCellStrategy : INewCellStrategy
        {
            public bool IsNewCell(ICellIndex cellIndex, PlayData playData)
            {
                var neighborsCount = playData.GetLifeNeighborsCount(cellIndex);
                return neighborsCount == 3 || neighborsCount == 6;
            }
        }

        private readonly int _cols;
        private readonly int _rows;
        private readonly char[,] _area;

        public PlayData(string[] data)
        {
            Dictionary<string, string> GetParams(ref int pos)
            {
                Dictionary<string, string> dataParams = new Dictionary<string, string>();

                while (pos < data.Length && !string.IsNullOrWhiteSpace(data[pos]))
                {
                    var parts = data[pos].Split(':');
                    dataParams.Add(parts[0], parts[1]);

                    pos++;
                }

                return dataParams;
            }

            void GetDimensions(ref int pos, out int cols, out int rows)
            {
                if (pos >= data.Length)
                {
                    cols = rows = 0;
                    return;
                }

                string[] parts = data[pos].Split(' ');
                cols = int.Parse(parts[0]);
                rows = int.Parse(parts[1]);
            }

            char[,] GetArea(ref int pos, int cols, int rows)
            {
                char[,] area = new char[cols, rows];

                int row = 0;
                while (pos < data.Length)
                {
                    int col = 0;
                    foreach (var c in data[pos])
                    {
                        area[col++, row] = c;
                    }

                    pos += 1;
                    row += 1;
                }

                return area;
            }

            Command = data[0];

            int i = 1;

            Params = GetParams(ref i);

            i += 1;

            GetDimensions(ref i, out _cols, out _rows);

            i += 1;

            _area = GetArea(ref i, _cols, _rows);
        }

        public IReadOnlyDictionary<string, string> Params;

        public string Command { get; }

        public int Cols => _cols;

        public int Rows => _rows;

        public IEnumerable<ICellIndex> GetCellIndexes() => GetCellIndexes(c => true);
        
        public IEnumerable<ICellIndex> GetCellIndexes(Func<ICellIndex, bool> filter)
        {
            for (int col = 0; col < _cols; col++)
            {
                for (int row = 0; row < _rows; row++)
                {
                    var cellIndex = new CellIndex(col, row, this);
                    if (filter(cellIndex))
                    {
                        yield return cellIndex;
                    }
                }
            }
        }

        private bool IsValidCoords(ICellIndex cellIndex) => cellIndex.Col >= 0 && cellIndex.Col < _cols && cellIndex.Row >= 0 && cellIndex.Row < _rows;

        public bool IsLifeCoords(ICellIndex cellIndex) => IsValidCoords(cellIndex) && (_area[cellIndex.Col, cellIndex.Row] == 'X' || _area[cellIndex.Col, cellIndex.Row] == 'x');

        private int GetLifeNeighborsCount(ICellIndex cellIndex)
        {
            int neighborsCount = 0;

            for (int deltaCol = -1; deltaCol <= 1; deltaCol++)
            {
                for (int deltaRow = -1; deltaRow <= 1; deltaRow++)
                {
                    if (deltaCol == 0 && deltaRow == 0)
                    {
                        continue;
                    }

                    if (IsLifeCoords(new CellIndex(cellIndex.Col + deltaCol, cellIndex.Row + deltaRow, this)))
                    {
                        neighborsCount++;
                    }
                }
            }

            return neighborsCount;
        }

        public bool IsSurvivingCell(ICellIndex cellIndex)
        {
            if (!IsLifeCoords(cellIndex))
            {
                return false;
            }

            var neighborsCount = GetLifeNeighborsCount(cellIndex);
            return neighborsCount == 2 || neighborsCount == 3;
        }

        private bool IsDyingCell(ICellIndex cellIndex)
        {
            if (!IsLifeCoords(cellIndex))
            {
                return false;
            }

            var neighborsCount = GetLifeNeighborsCount(cellIndex);
            return neighborsCount != 2 && neighborsCount != 3;
        }

        public bool IsNewCell(ICellIndex cellIndex, INewCellStrategy newCellStrategy)
        {
            return !IsLifeCoords(cellIndex) && newCellStrategy.IsNewCell(cellIndex, this);
        }

        public void WriteArea(string outFile)
        {
            using (TextWriter tw = new StreamWriter(outFile))
            {
                tw.WriteLine($"{_cols} {_rows}");
                
                for (int row = 0; row < _rows; row++)
                {
                    for (int col = 0; col < _cols; col++)
                    {
                        tw.Write(_area[col, row]);
                    }

                    tw.WriteLine();
                }
            }
        }

        public void SetNextGeneration()
        {
            string paramValue;
            if (!Params.TryGetValue("Rules", out paramValue))
            {
                paramValue = "Life";
            }

            INewCellStrategy newCellStrategy;
            if (paramValue == "Life")
            {
                newCellStrategy = new RegularNewCellStrategy();
            }
            else if (paramValue == "HighLife")
            {
                newCellStrategy = new HighLifeNewCellStrategy();
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(paramValue));
            }

            foreach (var cellIndex in GetCellIndexes())
            {
                if (IsDyingCell(cellIndex))
                {
                    cellIndex.Char = 'x';
                }
                else
                {
                    if (IsNewCell(cellIndex, newCellStrategy))
                    {
                        cellIndex.Char = 'n';
                    }
                }
            }

            foreach (var cellIndex in GetCellIndexes())
            {
                if (cellIndex.Char == 'x')
                {
                    cellIndex.Char = '.';
                }
                else if (cellIndex.Char == 'n')
                {
                    cellIndex.Char = 'X';
                }
            }
        }

    }
}