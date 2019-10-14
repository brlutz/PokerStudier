using System;
using System.Collections.Generic;
using System.Linq;
using PokerStudier;

namespace PokerStudier1.Models
{     
    public class HandsOverviewViewModel
    {

        public PaginationSettings Pagination {get;set;}

        public HandsOverviewViewModel(List<HandHistory> hh, Filter filter, PaginationSettings pagination)
        {
            this.Pagination = pagination;
            this.Hands = hh;
            this.Filters = filter;
            if(this.Filters.OrderByHeroEarnings == "Losses")
            {
                //this.Hands = this.Hands.OrderBy(x => Math.Abs(x.HeroEarnings)).Reverse().ToList();
            }
            this.Hands.Take(this.Pagination.PageSize);
        }

        public List<string> Actions = new List<string>();

        //public List<TotalResultsObject Hands = new List<TotalResultsObject>();
        public List<HandHistory> Hands = new List<HandHistory>();
        public Filter Filters {get;set;}
    }
}