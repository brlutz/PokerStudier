using System.Collections.Generic;
using System.IO;

namespace PokerStudier
{
    public class PokerAnalyser
    {
        private List<HandHistory> HandHistories;

        private HandClassification Classification;

        public PokerAnalyser(List<HandHistory> handHistories)
        {
            this.HandHistories = handHistories;
            Classification = new HandClassification(this.HandHistories);
            PrintClassification();
        }

        public void PrintClassification()
        {
            Classification.PrintClassification();
        }




        
    }
}