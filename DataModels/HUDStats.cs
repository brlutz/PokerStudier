using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerStudier.DataModels
{
    public class HUDStats
    {
        private List<HandHistory> handHistories;
        public decimal VPIP { get; set; }
        public decimal PFR { get; set; }

        public decimal AF { get; set; }

        public HUDStats(List<HandHistory> handHistories, string playerName)
        {
            this.handHistories = handHistories;
            this.VPIP = CalculateVPIP(this.handHistories, playerName);
            this.PFR = CalculatePFR(this.handHistories, playerName);
            this.AF = CalculateAF(this.handHistories, playerName);
        }

        private decimal CalculateAF(List<HandHistory> handHistories, string playerName)
        {
            decimal af = 0;
            int aggressiveCount = 0;
            int passiveCount = 0;
            int totalCount = 0;
            foreach (HandHistory hh in handHistories)
            {
                PlayerHandHistory phh = hh.PlayerHandHistories.Where(x => x.PlayerName == playerName).SingleOrDefault();
                if (phh is null)
                {
                    continue;
                }
                else
                {
                    totalCount++;
                    aggressiveCount += phh.Actions.Where(x => x.HandAction.Contains("Bet") || x.HandAction.Contains("Raise")).Count();
                    passiveCount += phh.Actions.Where(x => x.HandAction.Contains("Bet")).Count();
                    
                }


            }
            af = passiveCount == 0 ? 1000 : (decimal)aggressiveCount / (decimal)passiveCount;

            return Math.Round(af, 2);
        }

        private decimal CalculatePFR(List<HandHistory> handHistories, string playerName)
        {
            decimal pfr = 0;
            int totalHandsActive = 0;
            foreach (HandHistory hh in handHistories)
            {
                PlayerHandHistory phh = hh.PlayerHandHistories.Where(x => x.PlayerName == playerName).SingleOrDefault();
                if (phh is null)
                {
                    continue;
                }
                else
                {
                    totalHandsActive++;
                    if (phh.Actions.Exists(x => x.HandAction.Contains("BeforeFlop")  && !x.HandAction.Contains("Raise")))
                    {
                        pfr++;
                    }
                }
            }
            pfr = totalHandsActive == 0 ? 0 : pfr / totalHandsActive;

            return Math.Round(pfr, 2);
        }

        private decimal CalculateVPIP(List<HandHistory> handHistories, string playerName)
        {
            decimal vpip = 0;
            int totalHandsActive = 0;
            foreach (HandHistory hh in handHistories)
            {
                PlayerHandHistory phh = hh.PlayerHandHistories.Where(x => x.PlayerName == playerName).SingleOrDefault();
                if (phh is null)
                {
                    continue;
                }
                else
                {
                    totalHandsActive++;
                    if (phh.Actions.Exists(x => x.HandAction.Contains("BeforeFlop") && !x.HandAction.Contains("Fold")))
                    {
                        vpip++;
                    }
                }



            }
            vpip =  totalHandsActive == 0 ? 0 : vpip / totalHandsActive;

            return Math.Round(vpip, 2);
        }


    }
}