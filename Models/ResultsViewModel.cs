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

        public ResultsViewModel(Dictionary<string, ResultsObject> results, Filter filter)
        {

            this.Results = results;
            Filters = filter;

        }

        public string RequestId { get; set; }

        public Dictionary<string, ResultsObject> Results = new Dictionary<string, ResultsObject>();
        private PokerAnalyser Analyser;

        public string PositionFilter {get;set;}
    }
}