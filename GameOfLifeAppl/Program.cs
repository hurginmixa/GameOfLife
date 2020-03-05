using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLifeAppl.Strategies;

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

            Console.WriteLine($"inFile                   : {Path.GetFileName(inFile)}");
            Console.WriteLine($"Command                  : {playData.Command}");
            Console.WriteLine($"Generations              : {playData.Params.Generations}");
            Console.WriteLine($"NeighborhoodStrategyName : {playData.Params.NeighborhoodStrategyName}");
            Console.WriteLine($"RulesStrategyName        : {playData.Params.RulesStrategyName}");
            Console.WriteLine($"Count                    : {playData.Params.Count}");
            Console.WriteLine($"LifeNewBirth             : {playData.Params.LifeNewBirth.Combine(", ")}");
            Console.WriteLine($"LifeSurvivals            : {playData.Params.LifeSurvivals.Combine(", ")}");

            switch (playData.Command)
            {
                case "CalcFieldSize":
                {
                    File.WriteAllText(outFile, (playData.Area.Cols * playData.Area.Rows).ToString());
                    break;
                }
                case "CalcLiveCells":
                {
                    int count = playData.Area.GetCellIndexes(c => c.IsLifeCell).Count();
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
                    IRuleStrategy ruleStrategy = StrategiesFactory.GetRuleStrategy(playData);
                    INeighborhoodStrategy neighborhoodStrategy = StrategiesFactory.GetNeighborhoodStrategy(playData);
                    var maxGenerations = playData.Params.Generations;
                    
                    int count = playData.Area.GetCellIndexes().Count(c => { return c.IsLifeCell && !ruleStrategy.IsDyingCellPolicy(c, neighborhoodStrategy.GetLifeNeighborhoodCount(c), maxGenerations); });
                    
                    File.WriteAllText(outFile, count.ToString());
                    break;
                }
                case "CalcNewCells":
                {
                    IRuleStrategy ruleStrategy = StrategiesFactory.GetRuleStrategy(playData);
                    INeighborhoodStrategy neighborhoodStrategy = StrategiesFactory.GetNeighborhoodStrategy(playData);

                    int count = playData.Area.GetCellIndexes().Count(c => ruleStrategy.IsNewCellPolicy(c, neighborhoodStrategy.GetLifeNeighborhoodCount(c)));
                    
                    File.WriteAllText(outFile, count.ToString());
                    break;
                }
                case "NextGeneration":
                {
                    playData.MakeNextGenerations();

                    playData.Area.WriteArea(outFile, playData.Params.UseGenerations);
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
