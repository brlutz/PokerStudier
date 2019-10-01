using System;
using System.Collections.Generic;

namespace PokerStudier.DataModels
{
    public class HUDStats
    {
        private List<HandHistory> handHistories;
        public decimal VPIP {get;set;}
        public decimal PFR {get;set;}

        public HUDStats(List<HandHistory> handHistories)
        {
            this.handHistories = handHistories;
            this.VPIP = CalculateVPIP(this.handHistories);
            this.PFR = CalculatePFR(this.handHistories);
        }

        private decimal CalculatePFR(List<HandHistory> handHistories)
        {
            decimal pfr = 0;
            foreach(HandHistory hh in handHistories)
            {
                if(hh.Actions.Exists(x => x.Contains("BeforeFlop") && x.Contains("Raise")))
                {
                    pfr++;
                }
            }
            pfr = pfr / handHistories.Count;

            return Math.Round(pfr, 2);
        }

        private decimal CalculateVPIP(List<HandHistory> handHistories)
        {
            decimal vpip = 0;
            foreach(HandHistory hh in handHistories)
            {
                if(hh.Actions.Exists(x => x.Contains("BeforeFlop") && !x.Contains("Fold")))
                {
                    vpip++;
                }
            }
            vpip = vpip / handHistories.Count;

            return Math.Round(vpip, 2);
        }

        
    }
}