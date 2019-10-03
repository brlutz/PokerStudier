using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerStudier.DataModels
{
    public class HUDStats
    {
        private List<HandHistory> handHistories;
        public decimal VPIP {get;set;}
        public decimal PFR {get;set;}

        public decimal AF {get;set;}

        public HUDStats(List<HandHistory> handHistories)
        {
            this.handHistories = handHistories;
            this.VPIP = CalculateVPIP(this.handHistories);
            this.PFR = CalculatePFR(this.handHistories);
            this.AF = CalculateAF(this.handHistories);
        }

        private decimal CalculateAF(List<HandHistory> handHistories)
        {
            decimal af = 0;
            int aggressiveCount = 0;
            int passiveCount = 0;
            foreach(HandHistory hh in handHistories)
            {
                aggressiveCount += hh.Actions.Where(x=> x.Contains("Bet") || x.Contains("Raise")).Count();
                passiveCount += hh.Actions.Where(x=> x.Contains("Bet")).Count();
            }
            af = (decimal)aggressiveCount / (decimal)passiveCount;

            return Math.Round(af, 2);
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