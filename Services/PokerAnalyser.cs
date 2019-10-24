using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PokerStudier.DataModels;
using PokerStudier1.Models;

namespace PokerStudier
{
    public class PokerAnalyser
    {
        private List<HandHistory> HandHistories;

        private RangeChart RangeChart;

        public HUDStats HUDStats;





        public PokerAnalyser(List<HandHistory> handHistories, Filter f, string playerName)
        {
            this.HandHistories = handHistories;
            this.HandHistories = FilterHandHistories(this.HandHistories, playerName, f);
            RangeChart = new RangeChart(this.HandHistories, f, playerName);

            // GetStatsForRangeChart(RangeChart, playerName);
            this.HUDStats = new HUDStats(this.HandHistories, playerName);
        }



    private List<HandHistory> FilterHandHistories(List<HandHistory> handHistories, string playerName, Filter f)
    {

        for (int i = handHistories.Count - 1; i >= 0; i--)
        {
            if (f.Position != null)
            {
                if (handHistories[i].PlayerHandHistories.Find(x => x.PlayerName == playerName)?.Position != f.Position)
                {
                    handHistories.RemoveAt(i);
                }
            }
        }

        return handHistories;
    }





        public Dictionary<string, TotalResultsObject> GetResults()
        {
            return this.RangeChart.Results;
        }
    }
}