using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PokerStudier;
using PokerStudier1.Models;

namespace PokerStudier1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string position)
        {


            return View(GetWholeRangeAnaylsisModelGetter(position));
        }

        [Route("/Hand/{hand}", Name = "Hand")]
        public IActionResult Hand(string hand)
        {
            return View(GetHandAnaylsisModelGetter(hand));
            // return View(ModelGetter(position));
        }

        [Route("/Hands", Name = "Hands")]
        public IActionResult Hands(string hand)
        {
            return View(GetHandsAnaylsisModelGetter());
            // return View(ModelGetter(position));
        }

        private HandOverviewViewModel GetHandAnaylsisModelGetter(string hand)
        {
            List<string> textFiles = new List<string>() { "HH1.txt", "HH2.txt", "HH3.txt" };

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

        private HandsOverviewViewModel GetHandsAnaylsisModelGetter()
        {
            List<string> textFiles = new List<string>() { "HH1.txt", "HH2.txt", "HH3.txt" };

            PokerParser p = new PokerParser();
            foreach (string file in textFiles)
            {
                p.ReadInFile(file);
            }

            Filter f = new Filter(null, null );
            PokerAnalyser a = new PokerAnalyser(p.HandHistories, f);
            List<string> actionOptions = new List<string>();

            return new HandsOverviewViewModel(p.HandHistories, f, new PaginationSettings() {PageSize = 100});
        }


        private ResultsViewModel GetWholeRangeAnaylsisModelGetter(string position)
        {
            List<string> textFiles = new List<string>() { "HH1.txt", "HH2.txt", "HH3.txt" };

            PokerParser p = new PokerParser();
            foreach (string file in textFiles)
            {
                p.ReadInFile(file);
            }
            Filter f = new Filter(position, null);
            PokerAnalyser a = new PokerAnalyser(p.HandHistories, f);
            List<string> actionOptions = new List<string>();
            foreach(HandHistory hh in p.HandHistories)
            {
                actionOptions.AddRange(hh.Actions);
                actionOptions = actionOptions.Distinct().ToList();
            }
            List<string> sortedActionOptions = new List<string>();
            sortedActionOptions.AddRange(actionOptions.Where(x => x.Contains("BeforeFlop")).ToList());
            sortedActionOptions.AddRange(actionOptions.Where(x => !x.Contains("Before") && x.Contains("Flop")).ToList());
            sortedActionOptions.AddRange(actionOptions.Where(x => x.Contains("Turn")).ToList());
            sortedActionOptions.AddRange(actionOptions.Where(x => x.Contains("River")).ToList());



            f.ActionOptions = sortedActionOptions;


            return new ResultsViewModel(a.GetResults(), f);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
