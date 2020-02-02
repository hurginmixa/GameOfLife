using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeAppl
{
    public static class Program
    {
        static void Main(string[] args)
        {
            string inFile = args[0];
            string outFile = args[1];

            TestCode(inFile, outFile);
        }

        public static void TestCode(string inFile, string outFile)
        {
            PlayData playData = new PlayData(File.ReadAllLines(inFile));

            if (playData.Command == "CalcFieldSize")
            {
                File.WriteAllText(outFile, (playData.Cols * playData.Rows).ToString());
            }
            else if (playData.Command == "CalcLiveCells")
            {
                int count = 0;

                for (int col = 0; col < playData.Cols; col++)
                {
                    for (int row = 0; row < playData.Rows; row++)
                    {
                        if (playData.IsLifeCoords(col, row))
                        {
                            count += 1;
                        }
                    }
                }

                File.WriteAllText(outFile, count.ToString());
            }
            else if (playData.Command == "CalcParamsCount")
            {
                File.WriteAllText(outFile, playData.Params.Count.ToString());
            }
            else if (playData.Command == "CalcSurvivingCells")
            {
                int count = 0;

                for (int col = 0; col < playData.Cols; col++)
                {
                    for (int row = 0; row < playData.Rows; row++)
                    {
                        if (!playData.IsLifeCoords(col, row))
                        {
                            continue;
                        }

                        int neighborsCount = 0;

                        for (int deltaCol = -1; deltaCol <= 1; deltaCol++)
                        {
                            for (int deltaRow = -1; deltaRow <= 1; deltaRow++)
                            {
                                if (deltaCol == 0 && deltaRow == 0)
                                {
                                    continue;
                                }

                                if (playData.IsLifeCoords(col + deltaCol, row + deltaRow))
                                {
                                    neighborsCount++;
                                }
                            }
                        }

                        if (neighborsCount == 2 || neighborsCount == 3)
                        {
                            count += 1;
                        }
                    }
                }

                File.WriteAllText(outFile, count.ToString());
            }
        }
    }
}
