using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace HandComplier
{



    class Program
    {

        public static List<string> Ids = new List<string>();
        public static List<List<string>> RawHands = new List<List<string>>();

        static void Main(string[] args)
        {
            string[] folders = new string[] {
                "C:\\source\\PokerStudier\\HHs",
                "C:\\Users\\benry\\AppData\\Local\\PokerStars.USNJ\\HandHistory\\PlayTheBlues4U"
            };


            foreach (string path in folders)
            {
                if (File.Exists(path))
                {
                    // This path is a file
                    ProcessFile(path);
                    throw new NotImplementedException();
                }
                else if (Directory.Exists(path))
                {
                    // This path is a directory
                    ProcessDirectory(path);
                }
                else
                {
                    Console.WriteLine("{0} is not a valid file or directory.", path);
                }
            }

            WriteToFile();

            Console.WriteLine(RawHands.Count);



        }

        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {
            using (StreamReader file = new StreamReader(path))
            {
                int counter = 0;
                string ln;
                List<string> hand = null;
                while ((ln = file.ReadLine()) != null)
                {
                    if (ln.StartsWith("*********** #"))
                    {
                        continue;
                    }
                    else if (ln.StartsWith("PokerStars Hand #"))
                    {
                        if (hand != null)
                        {
                            RawHands.Add(hand);
                            
                        }


                        hand = new List<string>();

                    }

                    hand.Add(ln);

                    counter++;
                }
                file.Close();
            }
        }

        public static string GetHandNumber(string line)
        {
            string pattern = "#\\d*:";
            string match = Regex.Match(line, pattern).Value;
            return match.Substring(1, match.Length - 2);
        }

        public static void WriteToFile()
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\source\HandComplier\allHands.txt"))
            {
                foreach (List<string> rawHand in RawHands)
                {
                    string id = GetHandNumber(rawHand[0]);
                    if(Ids.Contains(id))
                    {
                        continue;
                    }

                    Ids.Add(id);

                    foreach (string line in rawHand)
                    {
                            file.WriteLine(line);
                    }
                }
            }
        }
    }
}
