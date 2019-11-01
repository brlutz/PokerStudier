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
        public decimal ThreeBetPF { get; set; }
        public decimal AF { get; set; }

        public int HandCount { get; set; }

        public decimal Winnings { get; set; }

        private int AggressiveCount { get; set; }
        private int PassiveCount { get; set; }
        private int VPIPHands { get; set; }

        private int PFRHands { get; set; }


        public HUDStats(List<HandHistory> handHistories, string playerName)
        {
            this.handHistories = handHistories;

            foreach (HandHistory hh in handHistories)
            {
                PlayerHandHistory phh = hh.PlayerHandHistories.Where(x => x.PlayerName == playerName).SingleOrDefault();

                if (!(phh is null))
                {
                    this.HandCount++;
                    CollectVPIPData(phh);
                    CollectPFRData(phh);
                    CollectAFData(phh);
                    Collect3BetPFData(phh);
                    this.Winnings += GetHandWinnigs(phh);
                }

            }

            this.AF = CalculateAF();
            this.VPIP = CalculateVPIP();
            this.PFR = CalculatePFR();
            this.ThreeBetPF = Calculate3BetPF();
        }

        private decimal Calculate3BetPF()
        {

            decimal threeBet = (this.HandCount == 0 ? 0 : (decimal)this.ThreeBetPF / (decimal)this.HandCount)*100;
            return Math.Round(threeBet, 2);
        
        }

        private void Collect3BetPFData(PlayerHandHistory phh)
        {
            if (phh.Actions.Exists(x => x.Round.Contains(HandActions.PreFlop) && x.HandAction.Contains(HandActions.Raise) && x.RaiseCount == 3))
            {
                this.ThreeBetPF++;
            }
        }

        private void CollectVPIPData(PlayerHandHistory phh)
        {
            if (phh.Actions.Exists(x => x.Round.Contains(HandActions.PreFlop) && !x.HandAction.Contains(HandActions.Fold)))
            {
                this.VPIPHands++;
            }

        }

        private decimal GetHandWinnigs(PlayerHandHistory phh)
        {
            return phh.Earnings;
        }

        private void CollectAFData(PlayerHandHistory phh)
        {
            this.AggressiveCount += phh.Actions.Where(x => x.HandAction.Contains(HandActions.Bet) || x.HandAction.Contains(HandActions.Raise)).Count();
            this.PassiveCount += phh.Actions.Where(x => x.HandAction.Contains(HandActions.Bet)).Count();
        }

        private decimal CalculateAF()
        {
            decimal af = this.PassiveCount == 0 ? 1000 : (decimal)this.AggressiveCount / (decimal)this.PassiveCount;
            return Math.Round(af, 2);
        }

        private decimal CalculatePFR()
        {
            decimal pfr = 0;
            pfr = this.PFRHands == 0 ? 0 : (decimal)this.PFRHands / (decimal)this.HandCount;
            return Math.Round(pfr, 2);
        }

        private void CollectPFRData(PlayerHandHistory phh)
        {
            if (phh.Actions.Exists(x => x.Round.Contains(HandActions.PreFlop) && x.HandAction.Contains(HandActions.Raise)))
            {
                this.PFRHands++;
            }
        }

        private decimal CalculateVPIP()
        {
            decimal vpip = this.HandCount == 0 ? 0 : (decimal)this.VPIPHands / (decimal)this.HandCount;

            return Math.Round(vpip, 2);
        }
    }
}