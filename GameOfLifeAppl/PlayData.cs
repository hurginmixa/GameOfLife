using System;
using System.Collections.Generic;
using System.IO;
using GameOfLifeAppl.Strategies;

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

            public bool IsLifeCell => Char == LifeCellChar || Char == DyingCellChar;
        }

        #endregion

        private const char LifeCellChar = 'X';
        private const char DyingCellChar = 'x';
        private const char NewCellChar = 'n';
        private const char EmptyCellChar = '.';

        private readonly char[,] _area;

        public PlayData(string[] data)
        {
            #region Dictionary<string, string> GetParams(ref int pos)

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

            #endregion

            #region void GetDimensions(ref int pos, out int cols, out int rows)

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

            #endregion

            #region char[,] GetArea(ref int pos, int cols, int rows)

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

            #endregion

            Command = data[0];

            int parsingRow = 1;

            Params = GetParams(ref parsingRow);

            parsingRow += 1;

            GetDimensions(ref parsingRow, out var colCount, out var rowCount);

            parsingRow += 1;

            _area = GetArea(ref parsingRow, colCount, rowCount);
        }

        public IReadOnlyDictionary<string, string> Params;

        public string Command { get; }

        public int Cols => _area.GetLength(0);

        public int Rows => _area.GetLength(1);

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

        public bool TryMakeCellIndex(int col, int row, out ICellIndex cellIndex)
        {
            cellIndex = col >= 0 && col < Cols && row >= 0 && row < Rows 
                ? new CellIndex(col, row, this) 
                : null;

            return cellIndex != null;
        }

        public void WriteArea(string outFile)
        {
            using (TextWriter tw = new StreamWriter(outFile))
            {
                tw.WriteLine($"{Cols} {Rows}");
                
                for (int row = 0; row < Rows; row++)
                {
                    for (int col = 0; col < Cols; col++)
                    {
                        tw.Write(_area[col, row]);
                    }

                    tw.WriteLine();
                }
            }
        }

        public void MakeNextGenerations()
        {
            IRuleStrategy ruleStrategy = StrategiesFactory.GetRuleStrategy(this);

            INeighborhoodStrategy neighborhood = StrategiesFactory.GetNeighborhoodStrategy(this);

            int stepCount = GetStepCount();

            for (int i = 0; i < stepCount; i++)
            {
                MakeNextGeneration(neighborhood, ruleStrategy);
            }
        }

        private void MakeNextGeneration(INeighborhoodStrategy neighborhood, IRuleStrategy ruleStrategy)
        {
            foreach (var cellIndex in GetCellIndexes())
            {
                var neighborhoodCount = neighborhood.GetLifeNeighborhoodCount(cellIndex);

                if (ruleStrategy.IsDyingCellPolicy(cellIndex, neighborhoodCount))
                {
                    cellIndex.Char = DyingCellChar;
                    continue;
                }

                if (ruleStrategy.IsNewCellPolicy(cellIndex, neighborhoodCount))
                {
                    cellIndex.Char = NewCellChar;
                    continue;
                }
            }

            foreach (var cellIndex in GetCellIndexes())
            {
                switch (cellIndex.Char)
                {
                    case DyingCellChar:
                        cellIndex.Char = EmptyCellChar;
                        continue;

                    case NewCellChar:
                        cellIndex.Char = LifeCellChar;
                        continue;
                }
            }
        }

        public string NeighborhoodStrategyName()
        {
            return Params.TryGetValue("Neighborhood", out string neighborhoodStrategyName) ? neighborhoodStrategyName : "Moore";
        }

        public string GetRulesStrategyName()
        {
            return Params.TryGetValue("Rules", out string rulesStrategyName) ? rulesStrategyName : "Life";
        }

        public int GetStepCount()
        {
            return Params.TryGetValue("Steps", out string txtStep) ? int.Parse(txtStep) : 1;
        }
    }
}