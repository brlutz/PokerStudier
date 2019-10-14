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

        public Dictionary<string, TotalResultsObject> Results = new Dictionary<string, TotalResultsObject>();

        public List<string> Cards = new List<string>()
        {
        "A","K","Q","J","T","9","8","7","6","5","4","3","2"
        };

        public PokerAnalyser(List<HandHistory> handHistories, Filter f, string playerName)
        {
            this.HandHistories = handHistories;
            RangeChart = new RangeChart(this.HandHistories, f, playerName);
            this.HandHistories = RangeChart.GetHandHistories();
            //this.HandHistories = FilterHandHistories(this.HandHistories, f);
            GetStatsForRangeChart(RangeChart, playerName);
            this.HUDStats = new HUDStats(this.HandHistories, playerName);
        }

        private List<HandHistory> FilterHandHistories(List<HandHistory> handHistories, Filter f)
        {
            /* 
            for (int i = handHistories.Count - 1; i >= 0; i--)
            {
                if (f.Position != null)
                {
                    if (handHistories[i].Position != f.Position)
                    {
                        handHistories.RemoveAt(i);
                    }
                }
            }
            */
            return handHistories;
        }

        private void GetStatsForRangeChart(RangeChart range, string playerName)
        {
            PopulateClassification();

            foreach (HandHistory hh in RangeChart.GetHandHistories())
            {
                PlayerHandHistory phh = hh.PlayerHandHistories.Where(x => x.PlayerName == playerName).SingleOrDefault();

                if (phh != null)
                {
                    string key = phh.HandType;
                    string position = phh.Position;
                    this.Results[key].TotalCount++;

                    if (phh.MoneyPutInPotTotal > 0 && !phh.WasBlindPaid())
                    {
                        this.Results[key].InvolvedCount++;
                    }

                    if (phh.Earnings > 0)
                    {
                        this.Results[key].WinCount++;
                    }
                }
            }
        }

        private void PopulateClassification()
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                for (int j = 0; j < Cards.Count; j++)
                {
                    string cardClass = "";
                    if (i < j)
                    {
                        cardClass = Cards[i] + Cards[j] + "s";
                    }
                    else if (i > j)
                    {
                        cardClass = Cards[j] + Cards[i] + "o";
                    }
                    else if (i == j)
                    {
                        cardClass = Cards[j] + Cards[i];
                    }

                    Results[cardClass] = new TotalResultsObject();
                }
            }
        }




        public Dictionary<string, TotalResultsObject> GetResults()
        {
            return this.Results;
        }





    }
}