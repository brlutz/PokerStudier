
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using PokerStudier1.Models;

namespace PokerStudier
{
    public class ViewModelService
    {
        /* 
        public HandsOverviewViewModel GetHandsAnaylsisModelGetter()
        {
            List<string> textFiles = GetHandHistoryFiles();
            PokerParser p = new PokerParser();
            foreach (string file in textFiles)
            {
                p.ReadInFile(file);
            }

            Filter f = new Filter(null, null, null, "Losses");
            PokerAnalyser a = new PokerAnalyser(p.HandHistories, f);
            List<string> actionOptions = new List<string>();

            return new HandsOverviewViewModel(p.HandHistories, f, new PaginationSettings() { PageSize = 100 });
        }
        */

        public List<string> GetHandHistoryFiles()
        {
            List<string> textFiles = new List<string>() { "HHs/HH1 copy.txt", "HHs/HH2.txt", "HHs/HH3.txt", "HHs/HH4.txt", "HHs/HH5.txt", "HHs/HH6.txt" };

            return textFiles;
        }
        public ResultsViewModel GetWholeRangeAnaylsisModelGetter(string playerName,string position)
        {
            List<string> textFiles = GetHandHistoryFiles();

            PokerParser p = new PokerParser();
            foreach (string file in textFiles)
            {
                p.ReadInFile(file);
            }
            Filter f = new Filter(position, null);
            PokerAnalyser a = new PokerAnalyser(p.HandHistories, f, playerName);
            List<Action> actionOptions = new List<Action>();
            foreach (HandHistory hh in p.HandHistories)
            {
                actionOptions.AddRange(hh.PlayerHandHistories.Where(x => x.PlayerName == playerName).SingleOrDefault()?.Actions);
                actionOptions = actionOptions.Distinct().ToList();
            }
            List<string> sortedActionOptions = new List<string>();
            //sortedActionOptions.AddRange(actionOptions.Where(x => x.HandAction.Contains("BeforeFlop")).ToList());
            //sortedActionOptions.AddRange(actionOptions.Where(x => !x.Contains("Before") && x.Contains("Flop")).ToList());
            //sortedActionOptions.AddRange(actionOptions.Where(x => x.Contains("Turn")).ToList());
            //sortedActionOptions.AddRange(actionOptions.Where(x => x.Contains("River")).ToList());



            f.ActionOptions = sortedActionOptions;


            return new ResultsViewModel(a.GetResults(), a.HUDStats, f);
        }

        /* 
        public HandOverviewViewModel GetHandAnaylsisModelGetter(string hand)
        {
            List<string> textFiles = GetHandHistoryFiles();

            PokerParser p = new PokerParser();
            foreach (string file in textFiles)
            {
                p.ReadInFile(file);
            }

            Filter f = new Filter(null, hand);
            PokerAnalyser a = new PokerAnalyser(p.HandHistories, f);
            List<string> actionOptions = new List<string>();

            return new HandOverviewViewModel(p.HandHistories, f);
        }
        */


    }
}