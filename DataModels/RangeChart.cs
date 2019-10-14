using System;
using System.Collections.Generic;
using System.IO;
using PokerStudier1.Models;

public class RangeChart
{
    private List<HandHistory> handHistories;





    public RangeChart(List<HandHistory> handHistories, Filter f, string playerName)
    {

        PutHandsIntoRangeChart(handHistories, playerName);
        this.handHistories = handHistories;

    }

    public List<HandHistory> GetHandHistories()
    {
        return this.handHistories;
    }



    private void PutHandsIntoRangeChart(List<HandHistory> handHistories, string playerName)
    {
        foreach (HandHistory hh in handHistories)
        {
            foreach (PlayerHandHistory phh in hh.PlayerHandHistories)
            {
                if (phh.PlayerName != playerName)
                {

                    if (phh.HoleCards == null || phh.HoleCards == "")
                    {
                        return;
                    }
                    //string position = GetPosition(hh.Hand.RawHand);
                }
            }


        }
    }

    private string GetPosition(string rawHand)
    {
        return rawHand;
    }



    

}