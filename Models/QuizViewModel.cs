using System;
using System.Collections.Generic;
using System.Linq;
using PokerStudier;

namespace PokerStudier.Models
{     
    public class QuizViewModel
    {
        public QuizViewModel()
        {

        }

        public List<string> Options = new List<string>()
        {
            PlayerOptions.Fold,
            PlayerOptions.Call,
            PlayerOptions.Raise+" 3x",
            PlayerOptions.Raise+" 5x"
        };
        public string PlayerHand {get;set;}

        public int PlayerPosition {get;set;}
        public int DealerPosition {get;set;}

        public int EnemyPosition {get;set;}
        public decimal EnemyBetSize {get;set;}

        public int Enemy2Position {get;set;}
        public decimal Enemy2BetSize {get;set;}

        public decimal Stakes {get;set;}
        public int BetCount {get;set;}

        public string Answer {get;set;}




    }
    
}