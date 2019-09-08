using System;
using System.Collections.Generic;

public class HandHistory
{
    public string HandNumber {get;set;}
    public string GameType {get;set;}
    public string NumberOfPlayer {get;set;}

    public string Stakes {get;set;}

    public string DateTime {get;set;}

    public string Button {get;set;}

    public Hand Hand {get;set;}

    public decimal HeroStartMoney {get;set;}
    public decimal HeroEndMoney {get;set;}

    public decimal HeroEarnings {
        get
        {
            return this.HeroEndMoney - this.HeroStartMoney;
        }
    }

    public string Position {get;set;}


}

public class Hand
{
    public Hand(string rawHand)
    {
        this.RawHand = rawHand;
    }
    public string RawHand {get;set;}
}

public class Player
{

}
