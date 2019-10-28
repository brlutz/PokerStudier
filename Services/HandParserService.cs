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

        hh.PlayerHandHistories = GetPlayerHandHistories(rawHand);
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
            string playerName = line.Substring(line.IndexOf(":") + 1, line.IndexOf("(") - line.IndexOf(":") - 1).Trim();
            PlayerHandHistory p = new PlayerHandHistory(playerName);
            int moneyStart = line.IndexOf("($") + 2;
            int moneyEnd = line.IndexOf(" in chips)");
            string moneyString = line.Substring(moneyStart, moneyEnd - moneyStart);
            decimal startMoney = Convert.ToDecimal(moneyString);
            p.StartMoney = startMoney;


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
        GetShowDownInfo(ref phh, rawHand);


        return phh;
    }

    private void GetShowDownInfo(ref List<PlayerHandHistory> phh, List<string> rawHand)
    {

        List<string> rawHandShort = rawHand.TakeLast(13).ToList();

        foreach (string line in rawHandShort)
        {
            if (line.StartsWith("*** SUMMARY ***"))
            {
                continue;
            }
            if (line.StartsWith("Seat ") && !line.Contains("(didn't bet)") && !line.Contains("collected ($") && !line.Contains("folded on") && !line.Contains("folded before"))
            {
                string playerName = "";
                if (line.Contains("(button)") || line.Contains("(small blind)") || line.Contains("(big blind)"))
                {
                    // Get the player name
                    playerName = line.Substring(line.IndexOf(":") + 1, line.IndexOf("(") - line.IndexOf(":") - 1).Trim();
                }
                else if(line.Contains("mucked ["))
                {
                    playerName = line.Substring(line.IndexOf(":") + 1, line.IndexOf("mucked [") - line.IndexOf(":") - 1).Trim();
                
                }
                else if(line.Contains("showed ["))
                {
                     playerName = line.Substring(line.IndexOf(":") + 1, line.IndexOf("showed [") - line.IndexOf(":") - 1).Trim();
                }
                
                if (playerName != "")
                {
                    string holeCards = GetHoleCards(line);
                    if (phh.Any(x => x.PlayerName == playerName))
                    {
                        phh.Single(x => x.PlayerName == playerName).HoleCards = holeCards;
                    }
                }
                else
                {
                    throw new NotImplementedException();
                        
                    
                }
            }

        }
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

        for (int i = 0; i < rawHand.Count(); i++)
        {
            string line = rawHand[i];

            if (line.StartsWith("Uncalled"))
            {
                int playerNameStart = line.IndexOf("returned to") + 11;

                string playerName = line.Substring(playerNameStart, line.Length - playerNameStart).Trim();
                int moneyStart = line.IndexOf("($") + 2;
                int moneyEnd = line.IndexOf(") ");
                string moneyString = line.Substring(moneyStart, moneyEnd - moneyStart);
                decimal returnedMoney = Convert.ToDecimal(moneyString);

                phh.Where(x => x.PlayerName == playerName).Single().ReturnedMoney = returnedMoney;
            }

            if (line.Contains("collected $"))
            {
                bool hasSidePot = false;
                if (line.Contains("side pot") || line.Contains("main pot")) { hasSidePot = true; }
                // all of this is garbage and should be regex
                int nameEnds = line.IndexOf("collected $");
                string playerName = line.Substring(0, nameEnds).Trim();
                string moneyString = line.Split(" ").Reverse().Skip(hasSidePot ? 3 : 2).FirstOrDefault().Replace("$", "");
                decimal winnings = Convert.ToDecimal(moneyString);
                phh.Where(x => x.PlayerName == playerName).Single().Winnings = winnings;
            }

            if (i == 0)
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
                Dictionary<string, List<Action>> actions = GetStreetActions(rawHand.Skip(i + 1).ToList(), HandActions.Flop);
                foreach (string key in actions.Keys)
                {
                    phh.Where(x => x.PlayerName == key).Single().Actions.AddRange(actions[key]);
                }
                continue;
            }
            else if (line.StartsWith("*** TURN ***"))
            {
                Dictionary<string, List<Action>> actions = GetStreetActions(rawHand.Skip(i + 1).ToList(), HandActions.Turn);
                foreach (string key in actions.Keys)
                {
                    phh.Where(x => x.PlayerName == key).Single().Actions.AddRange(actions[key]);
                }
                continue;
            }
            else if (line.StartsWith("*** RIVER *** "))
            {
                Dictionary<string, List<Action>> actions = GetStreetActions(rawHand.Skip(i + 1).ToList(), HandActions.River);
                foreach (string key in actions.Keys)
                {
                    phh.Where(x => x.PlayerName == key).Single().Actions.AddRange(actions[key]);
                }
                continue;
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
            if (line.Contains("has timed out")) { continue; }
            if (line.Contains("leaves the table")) { continue; }
            if (line.Contains("joins the table")) { continue; }
            if (line.Contains("is disconnected")) { continue; }
            if (line.Contains("is connected")) { continue; }
            if (line.Contains("collected $")) { continue; }
            if (line.Contains("said, \"")) { continue; }
            if (line.Contains("was removed from the table")) { continue; }
            if (line.Contains("doesn't show hand")) { continue; }
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

            List<Action> a = new List<Action>();
            if (!actions.TryGetValue(player, out a))
            {
                actions.Add(player, a);
                actions[player] = new List<Action>();
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
            int indexOfSplit = line.IndexOf(":");
            string actionStuffString = line.Substring(indexOfSplit + 1, line.Length - indexOfSplit - 1);
            string[] actionStuff = actionStuffString.Trim().Split(" ");

            a.RaiseAmount = Convert.ToDecimal(actionStuff[1].Replace("$", ""));
            a.TotalAmount = Convert.ToDecimal(actionStuff[3].Replace("$", ""));
        }
        else if (line.Contains(HandActions.Call.ToLower()))
        {
            a.HandAction = HandActions.Call;
            int indexOfSplit = line.IndexOf(":");
            string actionStuffString = line.Substring(indexOfSplit + 1, line.Length - indexOfSplit - 1);
            string[] actionStuff = actionStuffString.Trim().Split(" ");
            a.TotalAmount = Convert.ToDecimal(actionStuff[1].Replace("$", ""));
        }
        else if (line.Contains(HandActions.Check.ToLower()))
        {
            a.HandAction = HandActions.Check;
        }
        else if (line.Contains(HandActions.Bet.ToLower()))
        {
            a.HandAction = HandActions.Bet;
            int indexOfSplit = line.IndexOf(":");
            string actionStuffString = line.Substring(indexOfSplit + 1, line.Length - indexOfSplit - 1);
            string[] actionStuff = actionStuffString.Trim().Split(" ");
            a.TotalAmount = Convert.ToDecimal(actionStuff[1].Replace("$", ""));
        }


        if (a.HandAction is null)
        {
            throw new ArgumentNullException($"There should be some sort of action. Line: {line} ");
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
