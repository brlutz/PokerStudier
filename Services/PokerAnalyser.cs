using System;
using System.Collections.Generic;
using System.IO;
using PokerStudier1.Models;

namespace PokerStudier
{
    public class PokerAnalyser
    {
        private List<HandHistory> HandHistories;

        private HandClassification Classification;

        public Dictionary<string, ResultsObject> Results = new Dictionary<string, ResultsObject>();
        
        public List<string> Cards = new List<string>()
        {
        "A","K","Q","J","T","9","8","7","6","5","4","3","2"
        };

        public PokerAnalyser(List<HandHistory> handHistories, Filter f)
        {
            this.HandHistories = handHistories;
            Classification = new HandClassification(this.HandHistories, f);
            PopulateResultsContainter();

        }

        private void PopulateResultsContainter()
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

                    Results[cardClass] = new ResultsObject();
                }
            }
        }





        public Dictionary<string, ResultsObject> GetResults()
        {
            return this.Results;
        }




        
    }
}