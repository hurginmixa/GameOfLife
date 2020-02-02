using System.Collections.Generic;

namespace GameOfLifeAppl
{
    class PlayData
    {
        private readonly int _cols;
        private readonly int _rows;

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

            Area = GetArea(ref i, _cols, _rows);
        }

        public IReadOnlyDictionary<string, string> Params;

        public char[,] Area { get; }

        public string Command { get; }

        public int Cols => _cols;

        public int Rows => _rows;

        public bool IsValidCoords(int col, int row) => col >= 0 && col < _cols && row >= 0 && row < _rows;

        public bool IsLifeCoords(int col, int row)
        {
            return IsValidCoords(col, row) && Area[col, row] == 'X';
        }
    }
}