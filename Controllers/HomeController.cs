using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PokerStudier;
using PokerStudier.Models;

namespace PokerStudier.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string position, string playerName = "PlayTheBlues4U")
        {
            
            ViewModelService s = new ViewModelService();
            return View(s.GetWholeRangeAnaylsisModelGetter(playerName, position));
        }

        public IActionResult Player(string position, string playerName = "PlayTheBlues4U")
        {
            playerName = playerName.Trim();
            ViewModelService s = new ViewModelService();
            return View(s.GetWholeRangeAnaylsisModelGetter(playerName, position));
        }


        [Route("/Hand/{hand}", Name = "Hand")]
        public IActionResult Hand(string hand, string playerName = "PlayTheBlues4U")
        {     
            ViewModelService s = new ViewModelService();
            return View(s.GetHandAnaylsisModelGetter(playerName, hand));
        }

        [Route("/quiz", Name = "Quiz")]
        public IActionResult Quiz(string hand, string playerName = "PlayTheBlues4U")
        {     
            ViewModelService s = new ViewModelService();
            return View(s.QuizModelGetter(playerName, hand));
        }

        [Route("/Hands", Name = "Hands")]
        public IActionResult Hands(string hand, string playerName = "PlayTheBlues4U")
        {
            ViewModelService s = new ViewModelService();
            return View(s.GetHandsAnaylsisModelGetter(playerName));
        }

        [Route("/Players", Name = "Players")]
        public IActionResult Players(string name)
        {
            ViewModelService s = new ViewModelService();
            return View(s.PlayersModelGetter());
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
