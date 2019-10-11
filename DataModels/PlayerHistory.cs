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
    }

    public string PlayerName { get; set; }

    public string HandType { get; set; }

    public string HandNumber { get; set; }
    public string GameType { get; set; }
    public string NumberOfPlayer { get; set; }

    public string DateTime { get; set; }

    public string Button { get; set; }

    public Hand Hand { get; set; }

    public decimal PlayerStartMoney { get; set; }
    public decimal PlayerWinnings { get; set; }
    public decimal PlayerEndMoney { get; set; }

    public decimal PlayerMoneyPutInPotTotal { get; set; }


    public decimal PlayerEarnings
    {
        get
        {
            return this.PlayerWinnings - this.PlayerMoneyPutInPotTotal;
        }
    }

    public string Position { get; set; }

    public List<string> Actions { get; set; }

    public List<string> FlopTurnRiverCards {get;set;}
    public HandHistory ParseHand()
    {

        List<string> lines = RawHand;

        // Get hand history, game type and game stakes
        HandNumber = GetHandNumber(lines[0]);
        GameType = GetGameType(lines[0]);
        Stakes = GetGameStakes(lines[0]);
        Actions = GetActions(lines);
        FlopTurnRiverCards = GetFlopTurnRiverCards(lines);

        // Get Hero starting amount
        HeroStartMoney = GetHeroStartMoney(lines);
        HeroEndMoney = GetHeroEndMoney(lines);

        Hand = new Hand(GetHeroHand(lines));

        Position = GetHeroPosition(lines);

        return this;

    }

    private List<string> GetFlopTurnRiverCards(List<string> lines)
    {
        List<string> flopTurnRiver = new List<string>();

        foreach(string line in lines)
        {

            if (flopTurnRiver.Count < 1 && line.StartsWith("*** FLOP ***"))
            {
                string pattern = "\\[.{0,11}\\]";
                string match = Regex.Match(line, pattern).Value;
                match = match.Replace("[", "").Replace("]","");
                flopTurnRiver.AddRange(match.Split(" "));
                continue;
            }
            else if (flopTurnRiver.Count > 2 && line.StartsWith("*** TURN ***"))
            {
                string pattern = "\\[.{0,2}\\]";
                string match = Regex.Match(line, pattern).Value;
                match = match.Replace("[", "").Replace("]","");
                flopTurnRiver.Add(match);
                continue;
            }
            else if (flopTurnRiver.Count > 3 && line.StartsWith("*** RIVER *** "))
            {
                string pattern = "\\[.{0,2}\\]";
                string match = Regex.Match(line, pattern).Value;
                match = match.Replace("[", "").Replace("]","");
                flopTurnRiver.Add(match);
                continue;
            }

            
        }
        return flopTurnRiver;
    }

    private List<string> GetActions(List<string> lines)
    {
        List<string> actions = new List<string>();

        string street = "";
        for(int i = 0; i < lines.Count; i++)
        {

            // TODO: break this out in function with by ref boolean
            if (lines[i].StartsWith("*** HOLE CARDS ***") )
            {
                street = "BeforeFlop";
                actions.AddRange(GetStreetActions(lines.Skip(i+1).ToList(), street));
                continue;
            }

            if (lines[i].StartsWith("*** FLOP ***"))
            {
                street = "Flop";
                actions.AddRange(GetStreetActions(lines.Skip(i+1).ToList(), street));
                continue;
            }

            if (lines[i].StartsWith("*** TURN ***"))
            {
                street = "Turn";
                actions.AddRange(GetStreetActions(lines.Skip(i+1).ToList(), street));
                continue;
            }

            if (lines[i].StartsWith("*** RIVER *** "))
            {
                street = "River";
                actions.AddRange(GetStreetActions(lines.Skip(i+1).ToList(), street));
                continue;
            }


        }

        return actions;
    }

    private List<string> GetStreetActions(List<string> lines, string round)
    {
        List<string> actions = new List<string>();
        int raiseCount = 0;
        foreach (string line in lines)
        {
            if (line.StartsWith("***") || line.StartsWith("Uncalled")) { break; }

            if (line.Contains("raise") && !line.Contains(HeroName))
            {
                raiseCount++;
            }

            if (line.Contains(HeroName) && line.Contains("calls"))
            {
                if (round == HandActions.BeforeFlop && raiseCount == 0)
                {
                    actions.Add(HandActions.Limped + round);
                }
                else if (raiseCount == 1)
                {
                    actions.Add(HandActions.Limped + round);
                }
                else if (raiseCount > 1)
                {
                    actions.Add(HandActions.Call + raiseCount + "bet" + round);
                }

            }
            else if (line.Contains(HeroName) && line.Contains("raise"))
            {

                actions.Add(HandActions.Raised + raiseCount + "bet" + round);

            }
            else if (line.Contains(HeroName) && line.Contains("bet"))
            {

                actions.Add(HandActions.Bet + round);
            }
            else if (line.Contains(HeroName) && line.Contains("check"))
            {

                actions.Add(HandActions.Check + round);
            }
            else if (line.Contains(HeroName) && line.Contains("fold"))
            {
                if (raiseCount > 0)
                {
                    actions.Add(HandActions.Fold + "to" + raiseCount + "raise" + round);
                }
                else
                {
                    actions.Add(HandActions.Fold + round);
                }

            }


        }



        return actions;

    }

    private string GetHeroPosition(List<string> lines)
    {
        string button = "Seat #\\d";

        List<string> positionKeysBackward = new List<string> { "Button", "Cutoff", "Hijack", "Lojack", "SmallBlind", "BigBlind", };
        List<string> positionKeys = new List<string> { "Button", "SmallBlind", "BigBlind", "Cutoff", "Hijack", "Lojack" };
        string pattern = "Seat #\\d";
        string match = Regex.Match(lines[1], pattern).Value;
        button = match.Replace("#", "");

        List<int> positions = new List<int>();
        int buttonPosition = -1;
        int heroPosition = -1;
        for (int i = 2; i < 11; i++)
        {
            if (lines[i].StartsWith("Seat"))
            {
                int positionNumber = Convert.ToInt16(lines[i].Replace(":", "").Split(" ")[1]);

                positions.Add(positionNumber);

                if (lines[i].StartsWith(button))
                {
                    buttonPosition = positions.Count;
                }

                if (lines[i].Contains(HeroName))
                {
                    heroPosition = positions.Count;
                }
            }
        }

        // TODO: Fix bug here
        // if offset is 1-0, then you're sb
        positionKeys = positionKeys.Take(positions.Count).ToList();
        positionKeysBackward = positionKeysBackward.Take(positions.Count).ToList();
        string heroPositionString = "";
        int diffBetweenHeroAndButton = heroPosition - buttonPosition;


        if (diffBetweenHeroAndButton < 0)
        {

            heroPositionString = positionKeysBackward[Math.Abs(diffBetweenHeroAndButton)];
        }
        else
        {
            heroPositionString = positionKeys[diffBetweenHeroAndButton];
        }


        return heroPositionString;
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

        return HeroWinnings - HeroMoneyPutInPotTotal;
    }

    private decimal HeroFindWinnings(List<string> lines)
    {
        decimal winnings = 0;
        foreach (string line in lines)
        {
            string pattern = "PlayTheBlues4U collected \\$\\S*";
            string match = Regex.Match(line, pattern).Value;
            if (match == "") { continue; }
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
        return Convert.ToDecimal(match.Replace("Uncalled bet ($", "").Replace(")", ""));
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
