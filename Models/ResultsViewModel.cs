using System;
using System.Collections.Generic;
using PokerStudier;

namespace PokerStudier1.Models
{

    public class Filter
    {

        public Filter(string position)
        {
            this.Position = position;
        }
       public string Position {get;set;}
    }

        
    public class ResultsViewModel
    {

        public Filter Filters {get;set;}

        public ResultsViewModel(Dictionary<string, TotalResultsObject> results, Filter filter)
        {

            this.Results = results;
            Filters = filter;

        }

        public string RequestId { get; set; }

        public Dictionary<string, TotalResultsObject> Results = new Dictionary<string, TotalResultsObject>();

        public string PositionFilter {get;set;}
    }
}