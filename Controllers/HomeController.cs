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


             return View(ModelGetter(position));
        }


        private ResultsViewModel ModelGetter(string position)
        {
            List<string> textFiles = new List<string>(){ "HH1.txt", "HH2.txt", "HH3.txt"};

            PokerParser p = new PokerParser();
            foreach(string file in textFiles)
            {
                p.ReadInFile(file);
            }
            Filter f = new Filter(position);
            PokerAnalyser a = new PokerAnalyser(p.HandHistories, f);

            
            return new ResultsViewModel(a, f);
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
