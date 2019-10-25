
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
            //Console.WriteLine("Hello World!");

            // Read file using StreamReader. Reads file line by line  
            using (StreamReader file = new StreamReader(fileName))
            {
                int counter = 0;
                string ln;
                List<List<string>> hands = new List<List<string>>();
                List<string> hand = null;

                while ((ln = file.ReadLine()) != null)
                {
                    if (ln.StartsWith("*********** #"))
                    {
                        continue;
                    }

                    if (ln.StartsWith("PokerStars Hand #") && hand != null)
                    {
                        hands.Add(hand);
                        hand = new List<string>();
                    }
                    else if(ln.StartsWith("PokerStars Hand #") && hand == null)
                    {
                        hand = new List<string>();
                    }

                    hand.Add(ln);

                    counter++;
                }
                file.Close();
                
                this.RawHands.AddRange(hands);
            }

            ParseHands();
        }

        public void ParseHands(int? handsToParse = null)
        {

            HandParserService s = new HandParserService();
            int counter = 0;
            foreach (List<string> rawHand in this.RawHands)
            {
                if (!rawHand[0].Contains("Tournament"))
                {
                    HandHistory hh = new HandHistory(rawHand);
                    hh = s.ParseHand(rawHand);
                    if (!this.HandHistories.Exists(x => x.HandNumber == hh.HandNumber))
                    {
                        this.HandHistories.Add(hh);
                        counter++;
                    }

                    if (handsToParse.HasValue && counter > handsToParse.Value)
                    {
                        break;
                    }
                }
            }

        }

    }
}