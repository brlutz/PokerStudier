using System;
using System.Collections.Generic;
using PokerStudier;

namespace PokerStudier1.Models
{
    public class ResultsViewModel
    {

        public ResultsViewModel()
        {
            PopulateClassification();
        }

        public ResultsViewModel(PokerAnalyser a)
        {
            this.Analyser = a;
            this.Results = a.GetClassification();
        }

        public List<string> Cards = new List<string>()
    {
        "A","K","Q","J","T","9","8","7","6","5","4","3","2"
    };


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

                Results[cardClass] = new ResultsObject();
            }
        }
    }


        public string RequestId { get; set; }

        public Dictionary<string, ResultsObject> Results = new Dictionary<string, ResultsObject>();
        private PokerAnalyser Analyser;
    }
}