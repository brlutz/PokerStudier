using System;
using System.Collections.Generic;
using System.Linq;
using PokerStudier;

namespace PokerStudier.Models
{     
    public class HandsOverviewViewModel
    {

        public PaginationSettings Pagination {get;set;}

        
        public HandsOverviewViewModel(List<HandHistory> hh, string playerName, Filter filter, PaginationSettings pagination)
        {
            this.Pagination = pagination;
            this.Hands = hh;
            this.Filters = filter;
            if(this.Filters.OrderByHeroEarnings == "Losses")
            {
                // TODO: Flatten these hand models, we know what player we're looking for, no need to have to linq my way to get everything
                this.Hands = hh.Where(x => x.PlayerHandHistories.Exists(y =>
                 y.PlayerName == playerName)).OrderBy(x =>
                    Math.Abs(x.PlayerHandHistories.SingleOrDefault(y => y.PlayerName == playerName).Earnings)).Reverse().ToList();
            }
            this.Hands.Take(this.Pagination.PageSize);
        }

        public List<string> Actions = new List<string>();

        //public List<TotalResultsObject Hands = new List<TotalResultsObject>();
        public List<HandHistory> Hands = new List<HandHistory>();
        public Filter Filters {get;set;}
    }
}