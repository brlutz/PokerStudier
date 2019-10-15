using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class HandParserService
{

    public HandParserService()
    {

    }

    public HandHistory ParseHand(List<string> rawHand)
    {

        List<string> lines = rawHand;

        HandHistory hh = new HandHistory(rawHand);

        // Get hand history, game type and game stakes
        hh.HandNumber = GetHandNumber(lines[0]);
        hh.GameType = GetGameType(lines[0]);
        hh.Stakes = GetGameStakes(lines[0]);

        hh.FlopTurnRiverCards = GetFlopTurnRiverCards(lines);

        List<PlayerHandHistory> playerHandHistories = new List<PlayerHandHistory>();

        playerHandHistories = GetPlayerHandHistories(rawHand);
        //Actions = GetActions(lines);
        // Get Hero starting amount
        //HeroStartMoney = GetHeroStartMoney(lines);
        //HeroEndMoney = GetHeroEndMoney(lines);

         // = new Hand(GetHeroHand(lines));

        // Position = GetHeroPosition(lines);

        return hh;

    }

    private List<PlayerHandHistory> GetPlayerHandHistories(List<string> rawHand)
    {
        List<PlayerHandHistory> phh = new List<PlayerHandHistory>();

        phh.AddRange(GetPlayersAndPositions(rawHand));

        return phh;

    }

    private IEnumerable<PlayerHandHistory> GetPlayersAndPositions(List<string> rawHand)
    {
        List<PlayerHandHistory> phh = new List<PlayerHandHistory>();

        int playerStartLine = 2;
        int count = 0;
        string line = rawHand[playerStartLine + count];

        string pattern = "Seat #\\d";
        string match = Regex.Match(rawHand[1], pattern).Value;
        string buttonSeat = match.Replace("#", "");
        int buttonPosition = 0;

        while (line.StartsWith("Seat "))
        {
            // Get the player name
            string playerName = line.Substring(line.IndexOf(":") + 1, line.IndexOf("(")-line.IndexOf(":")-1).Trim();
            PlayerHandHistory p = new PlayerHandHistory(playerName);

            // get which player in the list is the button
            if (line.StartsWith(buttonSeat))
            {
                buttonPosition = phh.Count();
            }

            phh.Add(p);
            count++;
            line = rawHand[playerStartLine + count];
        }

        GetPlayerPositions(ref phh, buttonPosition);


        GetHandActions(ref phh, rawHand);
        

        return phh;
    }

    private void GetSmallBlind(ref List<PlayerHandHistory> phh, string line)
    {
        string smallBlindName = line.Split(';').First();
       
       // say where someone paid the blinds;
        // phh.Where(x => x.PlayerName == smallBlindName).Single().
    }

    private void GetBigBlind(ref List<PlayerHandHistory> phh, string line)
    {
        throw new NotImplementedException();
    }

    private void GetHandActions(ref List<PlayerHandHistory> phh, List<string> rawHand)
    {
        int lineCount = 0;
        foreach (string line in rawHand)
        {
            lineCount++;
            if (line.StartsWith("*** HOLE CARDS ***"))
            {
                break;
            }
            
        }

        // Get rid of already processed stuff
        rawHand = rawHand.Skip(lineCount).ToList();

        //Get Hole cards
        phh.Where(x => rawHand[0].Contains(x.PlayerName)).Single().HoleCards = GetHoleCards(rawHand[0]);
        rawHand = rawHand.Skip(1).ToList();
        
        for(int i = 0; i < rawHand.Count(); i++)
        {
            lineCount++;
            string line = rawHand[i];

            if (line.StartsWith("*** HOLE CARDS ***"))
            {
                Dictionary<string, List<Action>> actions = GetStreetActions(rawHand.Skip(i).ToList(), HandActions.PreFlop);
                foreach (string key in actions.Keys)
                {
                    phh.Where(x => x.PlayerName == key).Single().Actions.AddRange(actions[key]);
                }
                continue;
            }
            else if (line.StartsWith("*** FLOP ***"))
            {
                Dictionary<string, List<Action>> actions = GetStreetActions(rawHand.Skip(i).ToList(), HandActions.Flop);
                foreach (string key in actions.Keys)
                {
                    phh.Where(x => x.PlayerName == key).Single().Actions.AddRange(actions[key]);
                }
                continue;
            }
            else if (line.StartsWith("*** TURN ***"))
            {
                Dictionary<string, List<Action>> actions =  GetStreetActions(rawHand.Skip(i).ToList(), HandActions.Turn);
                foreach (string key in actions.Keys)
                {
                    phh.Where(x => x.PlayerName == key).Single().Actions.AddRange(actions[key]);
                }
                continue;
            }
            else if (line.StartsWith("*** RIVER *** "))
            {
                Dictionary<string, List<Action>> actions = GetStreetActions(rawHand.Skip(i).ToList(), HandActions.River);
                foreach (string key in actions.Keys)
                {
                    phh.Where(x => x.PlayerName == key).Single().Actions.AddRange(actions[key]);
                }
                break;
            }
        }
    }

    private string GetHoleCards(string line)
    {
        string pattern = "\\[\\S\\S \\S\\S]";
        string match = Regex.Match(line, pattern).Value;
        match = match.Replace("[", "").Replace("]", "");
        return match;

    }

    private void GetPlayerPositions(ref List<PlayerHandHistory> phh, int buttonPosition)
    {
        List<string> positionKeysBackward = new List<string> { "Button", "Cutoff", "Hijack", "Lojack", "SmallBlind", "BigBlind", };
        List<string> positionKeys = new List<string> { "Button", "SmallBlind", "BigBlind", "Cutoff", "Hijack", "Lojack" };
        positionKeys = positionKeys.Take(phh.Count).ToList();
        positionKeysBackward = positionKeysBackward.Take(phh.Count).ToList();

        for (int i = 0; i < phh.Count(); i++)
        {
            int diffBetweenPlayerAndButton = i - buttonPosition;

            if (diffBetweenPlayerAndButton < 0)
            {
                // bug?
                phh[i].Position = positionKeys[positionKeys.Count() + diffBetweenPlayerAndButton];
                //phh[i].Position = positionKeysBackward[Math.Abs(diffBetweenPlayerAndButton)];
            }
            else
            {
                phh[i].Position = positionKeys[diffBetweenPlayerAndButton];
            }

        }
    }

    private List<string> GetFlopTurnRiverCards(List<string> lines)
    {
        List<string> flopTurnRiver = new List<string>();

        foreach (string line in lines)
        {
            if (flopTurnRiver.Count < 1 && line.StartsWith("*** FLOP ***"))
            {
                string pattern = "\\[.{0,11}\\]";
                string match = Regex.Match(line, pattern).Value;
                match = match.Replace("[", "").Replace("]", "");
                flopTurnRiver.AddRange(match.Split(" "));
                continue;
            }
            else if (flopTurnRiver.Count > 2 && line.StartsWith("*** TURN ***"))
            {
                string pattern = "\\[.{0,2}\\]";
                string match = Regex.Match(line, pattern).Value;
                match = match.Replace("[", "").Replace("]", "");
                flopTurnRiver.Add(match);
                continue;
            }
            else if (flopTurnRiver.Count > 3 && line.StartsWith("*** RIVER *** "))
            {
                string pattern = "\\[.{0,2}\\]";
                string match = Regex.Match(line, pattern).Value;
                match = match.Replace("[", "").Replace("]", "");
                flopTurnRiver.Add(match);
                continue;
            }


        }
        return flopTurnRiver;
    }


    private Dictionary<string, List<Action>> GetStreetActions(List<string> lines, string round)
    {
        Dictionary<string, List<Action>> actions = new Dictionary<string, List<Action>>();
        int raiseCount = (round == HandActions.PreFlop ? 1 : 0);
        foreach (string line in lines)
        {
            if (line.StartsWith("***") || line.StartsWith("Uncalled")) { break; }

            string player = GetPlayerNameFromActionLine(line);
            Action action = GetPlayerActionFromActionLine(line);
            action.Round = round;
            action.RaiseCount = raiseCount;
            if (action.HandAction == HandActions.Raise)
            {
                raiseCount++;
            }
            else if (action.HandAction == HandActions.Call)
            {
                // swap check for limp if stuff is preflop
                if (raiseCount == 1 && round == HandActions.PreFlop)
                {
                    action.HandAction = HandActions.Limped;
                }
            }
            actions[player].Add(action);

        }

        return actions;
    }

    private Action GetPlayerActionFromActionLine(string line)
    {
        Action a = new Action();

        if (line.Contains(HandActions.Fold.ToLower()))
        {
            a.HandAction = HandActions.Fold;
        }
        else if (line.Contains(HandActions.Raise.ToLower()))
        {
            a.HandAction = HandActions.Raise;
            string[] actionStuff = line.Skip(line.IndexOf(":")).ToString().Trim().Split(" ");
            a.RaiseAmount = actionStuff[1];
            a.TotalAmount = actionStuff[3];
        }
        else if (line.Contains(HandActions.Call.ToLower()))
        {
            a.HandAction = HandActions.Call;
            string[] actionStuff = line.Skip(line.IndexOf(":")).ToString().Trim().Split(" ");
            a.TotalAmount = actionStuff[1];
        }
        else if (line.Contains(HandActions.Check.ToLower()))
        {
            a.HandAction = HandActions.Check;
        }

        return a;
    }

    private string GetPlayerNameFromActionLine(string line)
    {
        return line.Split(":")[0];
    }



    private decimal ParseReturned(string line)
    {
        string pattern = "Uncalled bet \\(\\$\\S*";
        string match = Regex.Match(line, pattern).Value;
        return Convert.ToDecimal(match.Replace("Uncalled bet ($", "").Replace(")", ""));
    }


    private decimal GetPlayerStartMoney(List<string> lines, string playerName)
    {

        for (int i = 2; i < lines.Count; i++)
        {
            string line = lines[i];

            if (line.StartsWith("***"))
            {
                break;
            }

            if (line.Contains(playerName))
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
