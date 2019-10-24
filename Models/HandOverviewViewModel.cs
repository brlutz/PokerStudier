using System;
using System.Collections.Generic;
using System.Linq;
using PokerStudier;

namespace PokerStudier1.Models
{     
    public class HandOverviewViewModel
    {
        public string Hand {get;set;}

        public HandOverviewViewModel(List<HandHistory> hh, string playerName, Filter filter)
        {
            if(filter.Hand != null)
            {
                this.Hands = hh.Where(x => x.PlayerHandHistories.Exists(y => y.PlayerName == playerName && y.HandType == filter.Hand)).ToList();
            }
            this.Hand = filter.Hand;
        }

        public List<string> Actions = new List<string>();

        //public List<TotalResultsObject Hands = new List<TotalResultsObject>();
        public List<HandHistory> Hands = new List<HandHistory>();
        public Filter Filters {get;set;}
    }
}