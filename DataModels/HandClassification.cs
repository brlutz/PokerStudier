using System;
using System.Collections.Generic;
using System.IO;
using PokerStudier1.Models;

public class HandClassification
{


    public List<string> Cards = new List<string>()
    {
        "A","K","Q","J","T","9","8","7","6","5","4","3","2"
    };


    private void PopulateClassification()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            for (int j = 0; j < Cards.Count; j++)
            {
                string cardClass = "";
                if (i < j)
                {
                    cardClass = Cards[i] + Cards[j] + "s";
                }
                else if (i > j)
                {
                    cardClass = Cards[j] + Cards[i] + "o";
                }
                else if (i == j)
                {
                    cardClass = Cards[j] + Cards[i];
                }

                Classification[cardClass] = new ResultsObject();
            }
        }
    }


    public HandClassification(List<HandHistory> handHistories, Filter f)
    {
        this.handHistories = handHistories;
        PopulateClassification();
        ClassifyHands(handHistories, f);
    }

    private void ClassifyHands(List<HandHistory> handHistories, Filter f)
    {
        foreach (HandHistory hh in handHistories)
        {
            if(f.Position != null)
            {
                if(hh.Position != f.Position) {continue;}
            }

            string key = ClassifyHand(hh.Hand.RawHand);
            string position = GetPosition(hh.Hand.RawHand);
            this.Classification[key].TotalCount++;

            if (hh.HeroMoneyPutInPotTotal > 0 && hh.BlindPaid != null)
            {
                this.Classification[key].InvolvedCount++;
            }

            if (hh.HeroEarnings > 0)
            {
                this.Classification[key].WinCount++;
            }

        }
    }

    private string GetPosition(string rawHand)
    {
        return rawHand;
    }

    public void PrintClassification()
    {
        /* 
        foreach (string key in this.Classification.Keys)
        {
            ResultsObject obj = this.Classification[key];
            if(obj.TotalCount > 0)
            {
                Console.WriteLine(key);
                Console.WriteLine("Total Hands: "+ obj.TotalCount);
                Console.WriteLine("Winning %: " +( (double)obj.WinCount/obj.TotalCount) * 100);
            }

        }
        */
    }

    public Dictionary<string, ResultsObject> Classification = new Dictionary<string, ResultsObject>();
    private List<HandHistory> handHistories;

    public string ClassifyHand(string rawHand)
    {
        string firstCard = rawHand[0].ToString();
        string firstSuit = rawHand[1].ToString();
        string secondCard = rawHand[3].ToString();
        string secondSuit = rawHand[4].ToString();
        string suitClassification = "";

        if (firstSuit == secondSuit)
        {
            suitClassification = "s";
        }
        else if (firstSuit != secondSuit && firstCard != secondCard)
        {
            suitClassification = "o";
        }

        foreach (string key in this.Classification.Keys)
        {
            if (firstCard + secondCard + suitClassification == key)
            {
                return key;
            }
            else if (secondCard + firstCard + suitClassification == key)
            {
                return key;
            }
        }

        throw new InvalidDataException();
    }

}