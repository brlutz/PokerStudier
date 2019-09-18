using System.Collections.Generic;
using System.IO;
using PokerStudier1.Models;

namespace PokerStudier
{
    public class PokerAnalyser
    {
        private List<HandHistory> HandHistories;

        private HandClassification Classification;

        public PokerAnalyser(List<HandHistory> handHistories, Filter f)
        {
            this.HandHistories = handHistories;
            Classification = new HandClassification(this.HandHistories, f);
            PrintClassification();
        }

        public void PrintClassification()
        {
            //Classification.Classification
        }




        public Dictionary<string, ResultsObject> GetClassification()
        {
            return Classification.Classification;
        }




        
    }
}