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
        public IActionResult Index()
        {

            const string textFile = "HH1.txt";
            PokerParser p = new PokerParser();
            p.ReadInFile(textFile);

            PokerAnalyser a = new PokerAnalyser(p.HandHistories);

             return View(new ResultsViewModel(a));
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
