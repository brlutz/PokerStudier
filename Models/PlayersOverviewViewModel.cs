using System;
using System.Collections.Generic;
using System.Linq;
using PokerStudier;

namespace PokerStudier.Models
{     
    public class PlayersOverviewViewModel
    {

        public PaginationSettings Pagination {get;set;}
        public List<PlayerOverviewStats> Players = new List<PlayerOverviewStats>();
        
        public PlayersOverviewViewModel(List<HandHistory> hh, PaginationSettings pagination)
        {
            this.Pagination = pagination;
            Dictionary<string, int> stats = new Dictionary<string, int>();

            foreach(HandHistory handHistory in hh)
            {
                foreach(PlayerHandHistory phh in handHistory.PlayerHandHistories)
                {
                    if(!stats.ContainsKey(phh.PlayerName))
                    {
                        stats.Add(phh.PlayerName, 0);
                    }
                    stats[phh.PlayerName]++;
                }
            }

            foreach(string player in stats.Keys)
            {
                Players.Add(new PlayerOverviewStats(player, stats[player]));
            }

            this.Players = this.Players.OrderByDescending(x => x.HandCount).ToList();
        }

        public List<string> Actions = new List<string>();

        //public List<TotalResultsObject Hands = new List<TotalResultsObject>();

        public Filter Filters {get;set;}
    }

    public class PlayerOverviewStats
    {

        public PlayerOverviewStats(string player, int handCount)
        {
            this.PlayerName = player;
            this.HandCount = handCount;
        }

        public string PlayerName {get;set;}
        public int HandCount {get;set;}
    }
}