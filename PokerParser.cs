
    using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PokerStudier
{
    public class PokerParser
    {
        public List<List<string>> RawHands = new List<List<string>>();
        public List<HandHistory> HandHistories = new List<HandHistory>();

        public void ReadInFile(string fileName)
        {
            const string textFile = "HH1.txt";
            //Console.WriteLine("Hello World!");

            // Read file using StreamReader. Reads file line by line  
            using (StreamReader file = new StreamReader(textFile))
            {
                int counter = 0;
                string ln;
                List<List<string>> hands = new List<List<string>>();
                List<string> hand = new List<string>();

                while ((ln = file.ReadLine()) != null)
                {
                    if (ln.Contains("***********") && counter > 5)
                    {
                        hands.Add(hand);
                        hand = new List<string>();
                    }
                    else if (!ln.Contains("***********"))
                    {
                        hand.Add(ln);
                    }

                    counter++;
                }
                file.Close();
                //Console.WriteLine($"File has {counter} lines.");

                this.RawHands = hands;
            }

            ParseHands();

            DisplayHands();

        }

        private void DisplayHands()
        {
            return;
            foreach(HandHistory hh in this.HandHistories)
            {
                Console.WriteLine("# "+hh.HandNumber);
                Console.WriteLine("Hero Played :" + hh.Hand.RawHand);
                Console.WriteLine("Hero started with : $"+ hh.HeroStartMoney.ToString());
                Console.WriteLine("Stakes are: " + hh.Stakes );
                Console.WriteLine("Big Blind is: $" + hh.BigBlind.ToString());
                Console.WriteLine("Small Blind is: $" + hh.SmallBlind.ToString());
                if(hh.BlindPaid != null){ Console.WriteLine("Paid the "+hh.BlindPaid);}
                Console.WriteLine("Hero Put in Pot: " + hh.HeroMoneyPutInPotTotal);
                Console.WriteLine("Hero won: $" + hh.HeroWinnings);
                Console.WriteLine("Hero diffed " + (hh.HeroWinnings - hh.HeroMoneyPutInPotTotal).ToString());
            }
        }

        public void ParseHands(int? handsToParse = null)
        {
            int counter = 0;
            foreach (List<string> rawHand in this.RawHands)
            {
                HandHistory hh = new HandHistory(rawHand, "PlayTheBlues4U");
                this.HandHistories.Add(hh.ParseHand());
                counter ++;

                if(handsToParse.HasValue && counter > handsToParse.Value)
                {
                    break;
                }
            }

        }

    }
}