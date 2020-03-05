using System;
using System.Linq;

namespace GameOfLifeAppl.Strategies
{
    internal static class StrategiesFactory
    {
        public static INeighborhoodStrategy GetNeighborhoodStrategy(PlayData playData)
        {
            string strategyName = playData.Params.NeighborhoodStrategyName;
            switch (strategyName)
            {
                case "Moore":
                    return new MooreNeighborhoodStrategy(playData);

                case "vonNeumann":
                    return new VonNeumannNeighborhoodStrategy(playData);

                default:
                    throw new ArgumentOutOfRangeException(nameof(strategyName));
            }
        }

        public static IRuleStrategy GetRuleStrategy(PlayData playData)
        {
            string strategyName = playData.Params.RulesStrategyName;
            switch (strategyName)
            {
                case "Life":
                {
                    var survivalsNeighborsCounts = playData.Params.LifeSurvivals;
                    
                    var newNeighborsCounts = playData.Params.LifeNewBirth;

                    return new ParametersRuleStrategy(survivalsNeighborsCounts: survivalsNeighborsCounts, newNeighborsCounts: newNeighborsCounts);
                }

                case "HighLife":
                    return new ParametersRuleStrategy(survivalsNeighborsCounts: new[] {2, 3}, newNeighborsCounts: new[] {3, 6});

                case "Diamoeba":
                    return new ParametersRuleStrategy(survivalsNeighborsCounts: new[] {5, 6, 7, 8}, newNeighborsCounts: new[] {3, 5, 6, 7, 8});

                default:
                    throw new ArgumentOutOfRangeException(nameof(strategyName));
            }
        }
    }
}
