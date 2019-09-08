using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class HandHistory
{

    public HandHistory(List<string> rawHandHistory, string heroName)
    {
        RawHand = rawHandHistory;
        HeroName = heroName;
    }

    private List<string> RawHand { get; set; }

    public string HeroName { get; set; }

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

    public string BlindPaid { get; internal set; }
    public string DateTime { get; set; }

    public string Button { get; set; }

    public Hand Hand { get; set; }

    public decimal HeroStartMoney { get; set; }
    public decimal HeroWinnings {get;set;}
    public decimal HeroEndMoney { get; set; }

    public decimal HeroMoneyPutInPotTotal { get; set; }

    public decimal Rake {get;set;}

    public decimal HeroEarnings
    {
        get
        {
            return this.HeroEndMoney - this.HeroStartMoney;
        }
    }

    public string Position { get; set; }


    public HandHistory ParseHand()
    {

        List<string> lines = RawHand;

        // Get hand history, game type and game stakes
        HandNumber = GetHandNumber(lines[0]);
        GameType = GetGameType(lines[0]);
        Stakes = GetGameStakes(lines[0]);


        // Get Hero starting amount
        HeroStartMoney = GetHeroStartMoney(lines);
        HeroEndMoney = GetHeroEndMoney(lines);

        Hand = new Hand(GetHeroHand(lines));

        return this;

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
        BlindPaid = FindIfBlindsPaid(lines);
        if (BlindPaid == "big") { HeroMoneyPutInPotTotal += BigBlind; }
        if (BlindPaid == "small") { HeroMoneyPutInPotTotal += SmallBlind; }

        HeroMoneyPutInPotTotal += FindBets();
        HeroWinnings += HeroFindWinnings(lines);

        return HeroWinnings - HeroMoneyPutInPotTotal ;
    }

    private decimal HeroFindWinnings(List<string> lines)
    {
        decimal winnings = 0;
        foreach (string line in lines)
        {
            string pattern = "PlayTheBlues4U collected \\$\\S*";
            string match = Regex.Match(line, pattern).Value;
            if(match == "") {continue;}
            winnings += Convert.ToDecimal(match.Replace("PlayTheBlues4U collected $", ""));
        }

        return winnings;
    }

    private decimal FindBets()
    {
        decimal sumOfMoney = 0;
        foreach (string line in RawHand)
        {
            if (line.Contains("PlayTheBlues4U: raises"))
            {
                sumOfMoney += ParseRaise(line);
                continue;
            }

            if (line.Contains("PlayTheBlues4U: calls"))
            {
                sumOfMoney += ParseCall(line);
                continue;
            }

            if (line.Contains("PlayTheBlues4U: bets"))
            {
                sumOfMoney += ParseBet(line);
                continue;
            }

            if (line.Contains("returned to PlayTheBlues4U"))
            {
                sumOfMoney -= ParseReturned(line);
            }

        }

        return sumOfMoney;
    }

    private decimal ParseReturned(string line)
    {
        string pattern = "Uncalled bet \\(\\$\\S*";
        string match = Regex.Match(line, pattern).Value;
        return Convert.ToDecimal(match.Replace("Uncalled bet ($", "").Replace(")",""));
    }

    private decimal ParseRaise(string line)
    {
        string pattern = "to \\$\\S*";
        string match = Regex.Match(line, pattern).Value;
        return Convert.ToDecimal(match.Replace("to $", ""));
    }

    private decimal ParseBet(string line)
    {
        string pattern = "bets \\$\\S*";
        string match = Regex.Match(line, pattern).Value;
        return Convert.ToDecimal(match.Replace("bets $", ""));
    }

    private decimal ParseCall(string line)
    {
        string pattern = "calls \\$\\S*";
        string match = Regex.Match(line, pattern).Value;
        return Convert.ToDecimal(match.Replace("calls $", ""));
    }

    private void FindWinnings(List<string> lines)
    {
        throw new NotImplementedException();
    }

    private string FindIfBlindsPaid(List<string> lines)
    {
        foreach (string line in lines)
        {
            if (line.StartsWith("*** HOLE CARDS ***"))
            {
                break;
            }
            else if (line.Contains("PlayTheBlues4U: posts"))
            {
                if (line.Contains("small"))
                {
                    return "small";
                }

                if (line.Contains("big"))
                {
                    return "big";
                }
            }
        }

        return null;
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
        string match = Regex.Match(line, pattern).Groups[0].Value;
        return match;
    }





}

public class Hand
{
    public Hand(string rawHand)
    {
        this.RawHand = rawHand;
    }
    public string RawHand { get; set; }
}

public class Player
{

}
