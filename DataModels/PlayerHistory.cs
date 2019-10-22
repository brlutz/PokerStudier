using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class PlayerHandHistory
{

    public PlayerHandHistory(string playerName)
    {
        PlayerName = playerName;
        this.Actions = new List<Action>();
    }


    public string PlayerName { get; set; }

    public string HandType
    {
        get
        {
            if(this.HoleCards == null || this.HoleCards == "")
            {
                return null;
            }

            if (this.handType == null || this.handType == "")
            {
                this.handType = ClassifyHandType(this.HoleCards);
            }
            return this.handType;
        }
    }

    private string ClassifyHandType(string rawHoleCards)
    {
        if (rawHoleCards == null || rawHoleCards == "")
        {
            throw new ArgumentNullException();
        }

        string firstCard = rawHoleCards[0].ToString();
        string firstSuit = rawHoleCards[1].ToString();
        string secondCard = rawHoleCards[3].ToString();
        string secondSuit = rawHoleCards[4].ToString();
        string suitClassification = "";

        if (firstSuit == secondSuit)
        {
            suitClassification = "s";
        }
        else if (firstSuit != secondSuit && firstCard != secondCard)
        {
            suitClassification = "o";
        }

        if (CardsHelper.Cards.IndexOf(firstCard) < CardsHelper.Cards.IndexOf(secondCard))
        {
            return firstCard + secondCard + suitClassification;
        }
        else
        {
            return secondCard + firstCard + suitClassification;
        }

        throw new InvalidDataException();
    }


    public bool WasBlindPaid()
    {
        return this.Position.ToLower().Contains("blind");
    }
    private string handType { get; set; }

    public string HoleCards { get; set; }

    public string HandNumber { get; set; }
    public string GameType { get; set; }
    public string NumberOfPlayer { get; set; }

    public string DateTime { get; set; }

    public string Button { get; set; }

    public Hand Hand { get; set; }

    public decimal StartMoney { get; set; }
    public decimal Winnings { get; set;}

    public decimal ReturnedMoney {get;set;}

    private decimal moneyPutInPotTotal = -1;
    public decimal MoneyPutInPotTotal { get
    {
        if(moneyPutInPotTotal >= 0)
        {
            return moneyPutInPotTotal;
        }

        moneyPutInPotTotal = 0;
        foreach(Action a in Actions)
        {
            moneyPutInPotTotal += a.TotalAmount;
        }

        moneyPutInPotTotal -= ReturnedMoney;
        return moneyPutInPotTotal;
    }
    }

    



    public decimal Earnings
    {
        get
        {
            if(this.MoneyPutInPotTotal < 0)
            {

            }
            return this.Winnings - this.MoneyPutInPotTotal;
        }
    }

    public string Position { get; set; }

    public List<Action> Actions { get; set; }
}
