using System.Collections.Generic;
using System.IO;
using GameOfLifeAppl.Strategies;

namespace GameOfLifeAppl
{
    internal class PlayData
    {
        public readonly PlayArea Area;

        public readonly ParamsCollection Params;

        public PlayData(string[] data)
        {
            #region Dictionary<string, string> GetParams(ref int pos)

            ParamsCollection GetParams(ref int pos)
            {
                Dictionary<string, string> dataParams = new Dictionary<string, string>();

                while (pos < data.Length && !string.IsNullOrWhiteSpace(data[pos]))
                {
                    var parts = data[pos].Split(':');
                    dataParams.Add(parts[0], parts[1]);

                    pos++;
                }

                return new ParamsCollection(dataParams);
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

            PlayArea GetArea(ref int pos, int cols, int rows)
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

                return new PlayArea(area);
            }

            #endregion

            Command = data[0];

            int parsingRow = 1;

            Params = GetParams(ref parsingRow);

            parsingRow += 1;

            GetDimensions(ref parsingRow, out var colCount, out var rowCount);

            parsingRow += 1;

            Area = GetArea(ref parsingRow, colCount, rowCount);
        }

        public string Command { get; }

        public void MakeNextGenerations()
        {
            IRuleStrategy ruleStrategy = StrategiesFactory.GetRuleStrategy(this);

            INeighborhoodStrategy neighborhood = StrategiesFactory.GetNeighborhoodStrategy(this);

            int stepCount = Params.StepCount;

            for (int i = 0; i < stepCount; i++)
            {
                MakeNextGeneration(neighborhood, ruleStrategy, Params.Generations);
            }
        }

        private void MakeNextGeneration(INeighborhoodStrategy neighborhood, IRuleStrategy ruleStrategy, int maxGenerations)
        {
            foreach (var cellIndex in Area.GetCellIndexes())
            {
                var neighborhoodCount = neighborhood.GetLifeNeighborhoodCount(cellIndex);

                if (ruleStrategy.IsDyingCellPolicy(cellIndex, neighborhoodCount, maxGenerations))
                {
                    cellIndex.SetDyingCell();
                    continue;
                }

                if (ruleStrategy.IsNewCellPolicy(cellIndex, neighborhoodCount))
                {
                    cellIndex.SetNewCell();
                    continue;
                }
            }

            Area.UpdateCells();
        }
    }
}