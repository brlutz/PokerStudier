using System;
using System.Collections.Generic;
using System.IO;
using PokerStudier1.Models;

namespace PokerStudier
{
    public class PokerAnalyser
    {
        private List<HandHistory> HandHistories;

        private HandClassifier Classification;

        public Dictionary<string, TotalResultsObject> Results = new Dictionary<string, TotalResultsObject>();

        public List<string> Cards = new List<string>()
        {
        "A","K","Q","J","T","9","8","7","6","5","4","3","2"
        };

        public PokerAnalyser(List<HandHistory> handHistories, Filter f)
        {
            this.HandHistories = handHistories;
            Classification = new HandClassifier(this.HandHistories, f);
            this.HandHistories = Classification.GetClassifiedHandHistories();
            this.HandHistories = FilterHandHistories(this.HandHistories, f);
            GetStatsForClassification(Classification);
        }

        private List<HandHistory> FilterHandHistories(List<HandHistory> handHistories, Filter f)
        {
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

            return handHistories;
        }

        private void GetStatsForClassification(HandClassifier classification)
        {
            PopulateClassification();
            foreach (HandHistory hh in Classification.GetClassifiedHandHistories())
            {
                string key = hh.HandType;
                string position = hh.Position;
                this.Results[key].TotalCount++;

                if (hh.HeroMoneyPutInPotTotal > 0 && hh.BlindPaid != null)
                {
                    this.Results[key].InvolvedCount++;
                }

                if (hh.HeroEarnings > 0)
                {
                    this.Results[key].WinCount++;
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