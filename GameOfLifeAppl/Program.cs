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
                int count = playData.GetCellIndexes(c => playData.IsLifeCoords(c)).Count();

                File.WriteAllText(outFile, count.ToString());
            }
            else if (playData.Command == "CalcParamsCount")
            {
                File.WriteAllText(outFile, playData.Params.Count.ToString());
            }
            else if (playData.Command == "CalcSurvivingCells")
            {
                int count = playData.GetCellIndexes(c => playData.IsLifeCoords(c)).Select(c => playData.GetLifeNeighborsCount(c)).Count(n => n == 2 || n == 3);

                File.WriteAllText(outFile, count.ToString());
            }
            else if (playData.Command == "CalcNewCells")
            {
                int count = playData.GetCellIndexes(c => !playData.IsLifeCoords(c)).Select(c => playData.GetLifeNeighborsCount(c)).Count(n => n == 3);

                File.WriteAllText(outFile, count.ToString());
            }
        }
    }
}
