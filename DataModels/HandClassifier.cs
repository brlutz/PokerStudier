using System;
using System.Collections.Generic;
using System.IO;
using PokerStudier1.Models;

public class HandClassifier
{
    private List<HandHistory> handHistories;

    public List<string> Cards = new List<string>()
    {
        "A","K","Q","J","T","9","8","7","6","5","4","3","2"
    };




    public HandClassifier(List<HandHistory> handHistories, Filter f)
    {

        ClassifyHands(handHistories);
        this.handHistories = handHistories;

    }

    public List<HandHistory> GetClassifiedHandHistories()
    {
        return this.handHistories;
    }



    private void ClassifyHands(List<HandHistory> handHistories)
    {
        foreach (HandHistory hh in handHistories)
        {
            hh.HandType = ClassifyHandType(hh.Hand.RawHand);
            string position = GetPosition(hh.Hand.RawHand);
        }
    }

    private string GetPosition(string rawHand)
    {
        return rawHand;
    }



    public string ClassifyHandType(string rawHand)
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

        if(Cards.IndexOf(firstCard) < Cards.IndexOf(secondCard))
        {
            return firstCard + secondCard + suitClassification;
        }
        else
        {
            return secondCard + firstCard + suitClassification;
        }

        throw new InvalidDataException();
    }

}