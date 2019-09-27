using System;
using System.Collections.Generic;
using PokerStudier;

namespace PokerStudier1.Models
{

    public class Filter
    {

        public Filter(string position, string hand, List<string> actionOptions = null)
        {
            this.Position = position;
            this.Hand = hand;

            if(actionOptions !=  null)
            {
                this.ActionOptions = actionOptions;
            }
        }
       public string Position {get;set;}

       public List<string> ActionOptions {get;set;}

       public string Hand {get;set;}
    }

        
    public class ResultsViewModel
    {



        public ResultsViewModel(Dictionary<string, TotalResultsObject> results, Filter filter)
        {

            this.Results = results;
            Filters = filter;
            Actions = filter.ActionOptions;

        }

        public string RequestId { get; set; }

        public Dictionary<string, TotalResultsObject> Results = new Dictionary<string, TotalResultsObject>();
        public Filter Filters {get;set;}

        public List<string> Actions = new List<string>();
        public string PositionFilter {get;set;}
    }
}