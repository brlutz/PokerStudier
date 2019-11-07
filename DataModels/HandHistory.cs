using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class HandHistory
{


    public HandHistory(List<string> rawHandHistory)
    {
        RawHand = rawHandHistory;
    }

    public List<string> RawHand { get; set; }

    public string HeroName { get; set; }

    public string HandType { get; set; }

    public string HandNumber { get; set; }
    public string GameType { get; set; }
    public string NumberOfPlayer { get; set; }

    private string stakes;

    public string Stakes
    {
        get
        {
            return stakes;
        }
        set
        {
            stakes = value;

            this.BigBlind = Convert.ToDecimal(Stakes.Split("/")[1].Replace("$", ""));
            this.SmallBlind = Convert.ToDecimal(Stakes.Split("/")[0].Replace("$", ""));
        }
    }


    public decimal BigBlind { get; internal set; }
    public decimal SmallBlind { get; internal set; }

    public string DateTime { get; set; }

    public string Button { get; set; }

    public decimal Rake { get; set; }

    public decimal TotalPot {get;set;}

    public List<PlayerHandHistory> PlayerHandHistories = new List<PlayerHandHistory>();

    public List<string> FlopTurnRiverCards {get;set;}
}

public class Hand
{
    public Hand(string rawHand)
    {
        this.RawHand = rawHand;
    }
    public string RawHand { get; set; }
}