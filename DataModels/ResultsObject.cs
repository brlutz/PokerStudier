public class ResultsObject
{
    public int TotalCount { get; set; }
    public int WinCount { get; set; }

    public int InvolvedCount { get; set; }

    public decimal WinPercent {
        get{
            if(TotalCount == 0){ return 0;}
            return decimal.Round((decimal)WinCount/TotalCount,1);
    }}
}