using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PokerStudier
{
    class Program
    {
        static void Main(string[] args)
        {
            const string textFile = "HH1.txt";
            PokerParser p = new PokerParser();
            p.ReadInFile(textFile);
        }
    }

    public class PokerParser
    {
        public const string HeroName = "PlayTheBlues4U";
        public List<List<string>> RawHands = new List<List<string>>();
        public List<HandHistory> HandHistories = new List<HandHistory>();

        public void ReadInFile(string fileName)
        {
            const string textFile = "HH1.txt";
            Console.WriteLine("Hello World!");

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

            ParseHands(15);

            DisplayHands();

        }

        private void DisplayHands()
        {
            foreach(HandHistory hh in this.HandHistories)
            {
                Console.WriteLine("# "+hh.HandNumber);
                Console.WriteLine("Hero Played :" + hh.Hand.RawHand);
                Console.WriteLine("Hero started with : $"+ hh.HeroStartMoney.ToString());
            }
        }

        public void ParseHands(int? handsToParse = null)
        {
            int counter = 0;
            foreach (List<string> rawHand in this.RawHands)
            {
                this.HandHistories.Add(ParseHand(rawHand));
                counter ++;

                if(handsToParse.HasValue && counter > handsToParse.Value)
                {
                    break;
                }
            }

        }

        public HandHistory ParseHand(List<string> hand)
        {
            HandHistory hh = new HandHistory();
            List<string> lines = hand;

            // Get hand history, game type and game stakes
            hh.HandNumber = GetHandNumber(lines[0]);
            hh.GameType = GetGameType(lines[0]);
            hh.Stakes = GetGameStakes(lines[0]);


            // Get Hero starting amount
            hh.HeroStartMoney = GetHeroStartMoney(lines);
            //hh.HeroEndMoney = GetHeroEndMoney(lines);

            hh.Hand = new Hand(GetHeroHand(lines));

            return hh;

        }

        private string GetHeroHand(List<string> lines)
        {
            for (int i = 5; i < lines.Count; i++)
            {
                string line = lines[i];

                if (line.StartsWith("*** HOLE CARDS ***"))
                {
                    line = lines[i + 1];
                    if (line.Contains(HeroName))
                    {
                        string pattern = "\\[\\S\\S \\S\\S]";
                        string match = Regex.Match(line, pattern).Value;
                        match = match.Replace("[", "").Replace("]", "");
                        return match;
                    }
                }

            }

            throw new InvalidDataException();
        }

        private decimal GetHeroEndMoney(List<string> lines)
        {
            throw new NotImplementedException();
        }

        private decimal GetHeroStartMoney(List<string> lines)
        {

            for (int i = 2; i < lines.Count; i++)
            {
                string line = lines[i];

                if (line.StartsWith("***"))
                {
                    break;
                }

                if (line.Contains(HeroName))
                {
                    string pattern = "(\\$\\S*)";
                    string match = Regex.Match(line, pattern).Value;
                    match = match.Replace("$", "");
                    return Convert.ToDecimal(match);
                }

            }

            throw new InvalidDataException();
        }

        public string GetHandNumber(string line)
        {
            string pattern = "#\\d*:";
            string match = Regex.Match(line, pattern).Value;
            return match.Substring(1, match.Length - 2);
        }

        public string GetGameType(string line)
        {
            if (line.Contains("Hold'em No Limit"))
            {
                return "Hold'em No Limit";
            }

            throw new ArgumentOutOfRangeException();
        }

        public string GetGameStakes(string line)
        {
            string pattern = "(\\$\\S*)";
            string match = Regex.Match(line, pattern).Value;
            return match;
        }



    }
}

