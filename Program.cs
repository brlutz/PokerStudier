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

            PokerAnalyser a = new PokerAnalyser(p.HandHistories);
            
        }
    }

}

