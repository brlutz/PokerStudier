using System;

public class TotalResultsObject
{
    public int TotalCount { get; set; }
    public int WinCount { get; set; }
    public int InvolvedCount { get; set; }



    private decimal? _winDecimal = null;
    public decimal WinDecimal {
        get{
            if(TotalCount == 0){ return 0;}
            if(!_winDecimal.HasValue){ _winDecimal = decimal.Round((decimal)WinCount/TotalCount,3);}
            return _winDecimal.Value;
    }}

        public decimal WinPercent {
        get{
            return Math.Round((WinDecimal * (decimal)100), 1);
    }}


}