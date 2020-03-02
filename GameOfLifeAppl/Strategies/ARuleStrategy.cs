using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLifeAppl.Strategies
{
    internal abstract class ARuleStrategy : IRuleStrategy
    {
        public bool IsNewCellPolicy(ICellIndex cellIndex, int neighborhoodCount)
        {
            return !cellIndex.IsLifeCell && CheckNeighborsCountForNewCell(cellIndex, neighborhoodCount);
        }

        protected abstract bool CheckNeighborsCountForNewCell(ICellIndex cellIndex, int neighborhoodCount);

        public bool IsDyingCellPolicy(ICellIndex cellIndex, int neighborhoodCount)
        {
            return cellIndex.IsLifeCell && !CheckNeighborsCountForSurvivals(cellIndex, neighborhoodCount);
        }

        protected abstract bool CheckNeighborsCountForSurvivals(ICellIndex cellIndex, int neighborhoodCount);

        public static IRuleStrategy GetStrategy(string strategyName1, PlayData playData)
        {
            string strategyName = playData.GetRulesStrategyName();
            switch (strategyName)
            {
                case "Life":
                {
                    var survivalsNeighborsCounts = new[] {2, 3};
                    
                    var newNeighborsCounts = new[] {3};

                    if (playData.Params.TryGetValue("Survivals", out string data))
                    {
                        survivalsNeighborsCounts = data.Split(',').Select(int.Parse).ToArray();
                    }

                    if (playData.Params.TryGetValue("NewBirth", out data))
                    {
                        newNeighborsCounts = data.Split(',').Select(int.Parse).ToArray();
                    }

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