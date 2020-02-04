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

            switch (playData.Command)
            {
                case "CalcFieldSize":
                {
                    File.WriteAllText(outFile, (playData.Cols * playData.Rows).ToString());
                    break;
                }
                case "CalcLiveCells":
                {
                    int count = playData.GetCellIndexes(c => playData.IsLifeCoords(c)).Count();
                    File.WriteAllText(outFile, count.ToString());
                    break;
                }
                case "CalcParamsCount":
                {
                    File.WriteAllText(outFile, playData.Params.Count.ToString());
                    break;
                }
                case "CalcSurvivingCells":
                {
                    int count = playData.GetCellIndexes().Count(c => playData.IsSurvivingCell(c));
                    File.WriteAllText(outFile, count.ToString());
                    break;
                }
                case "CalcNewCells":
                {
                    int count = playData.GetCellIndexes().Count(c => playData.IsNewCell(c));
                    File.WriteAllText(outFile, count.ToString());
                    break;
                }
                case "NextGeneration":
                {
                    foreach (var cellIndex in playData.GetCellIndexes())
                    {
                        if (playData.IsDyingCell(cellIndex))
                        {
                            cellIndex.Char = 'x';
                        }
                        else if (playData.IsNewCell(cellIndex))
                        {
                            cellIndex.Char = 'n';
                        }
                    }

                    foreach (var cellIndex in playData.GetCellIndexes())
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

                    playData.WriteArea(outFile);
                    break;
                }
            }
        }
    }
}
