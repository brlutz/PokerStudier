using System;
using System.Collections.Generic;
using PokerStudier;
using PokerStudier.DataModels;

namespace PokerStudier1.Models
{

    public class Filter
    {
        public Filter(string position, string hand, List<string> actionOptions = null, string orderByHeroEarnings = null)
        {
            this.Position = position;
            this.Hand = hand;
            this.OrderByHeroEarnings = orderByHeroEarnings;

            if(actionOptions !=  null)
            {
                this.ActionOptions = actionOptions;
            }
        }
       public string Position {get;set;}

       public string OrderByHeroEarnings {get;set;}

       public List<string> ActionOptions {get;set;}

       public string Hand {get;set;}
    }

        
    public class ResultsViewModel
    {



        public ResultsViewModel(Dictionary<string, TotalResultsObject> results, HUDStats hudStats, Filter filter)
        {

            this.Results = results;
            Filters = filter;
            Actions = filter.ActionOptions;
            this.HUDStats = hudStats;

        }

        public string RequestId { get; set; }

        public Dictionary<string, TotalResultsObject> Results = new Dictionary<string, TotalResultsObject>();
        public Filter Filters {get;set;}

        public List<string> Actions = new List<string>();
        public string PositionFilter {get;set;}

        public HUDStats HUDStats {get;set;}


    }
}