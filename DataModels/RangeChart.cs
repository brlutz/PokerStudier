using System;
using System.Collections.Generic;
using System.IO;
using PokerStudier1.Models;

public class RangeChart
{
    private List<HandHistory> handHistories;

    public List<string> Cards = new List<string>()
        {
        "A","K","Q","J","T","9","8","7","6","5","4","3","2"
        };


    public Dictionary<string, TotalResultsObject> Results = new Dictionary<string, TotalResultsObject>();

    public RangeChart(List<HandHistory> handHistories, Filter f, string playerName)
    {
        PutHandsIntoRangeChart(handHistories, playerName);
        this.handHistories = handHistories;

    }

    public List<HandHistory> GetHandHistories()
    {
        return this.handHistories;
    }

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

                Results[cardClass] = new TotalResultsObject();
            }
        }
    }


    private void PutHandsIntoRangeChart(List<HandHistory> handHistories, string playerName)
    {
        PopulateClassification();
        foreach (HandHistory hh in handHistories)
        {
            foreach (PlayerHandHistory phh in hh.PlayerHandHistories)
            {
                if (phh.PlayerName == playerName)
                {
                    if (phh.HoleCards == null || phh.HoleCards == "")
                    {
                        continue;
                    }
                    else
                    {
                        string key = phh.HandType;
                        string position = phh.Position;
                        this.Results[key].TotalCount++;

                        // TODO: Fix blinds
                        if (phh.MoneyPutInPotTotal > hh.BigBlind || phh.ReturnedMoney > 0)//)
                        {
                            this.Results[key].InvolvedCount++;

                            if (phh.Earnings > 0 || phh.ReturnedMoney > 0)
                            {
                                this.Results[key].WinCount++;
                            }
                        }


                    }
                }
            }


        }
    }

    private string GetPosition(string rawHand)
    {
        return rawHand;
    }





}