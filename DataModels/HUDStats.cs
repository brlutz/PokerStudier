using System;
using System.Collections.Generic;

namespace PokerStudier.DataModels
{
    public class HUDStats
    {
        private List<HandHistory> handHistories;

        public HUDStats(List<HandHistory> handHistories)
        {
            this.handHistories = handHistories;
            this.VPIP = CalculateVPIP(this.handHistories);
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

        public decimal VPIP {get;set;}
    }
}