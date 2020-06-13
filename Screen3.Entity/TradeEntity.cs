using System;

namespace Screen3.Entity
{
    public class TradeEntity
    {
        public string Id {get;set;}
        public String Code { get; set; }
        public int Direction { get; set; }
        public int EntryDate { get; set; }
        public double EntryPrice { get; set; }
        public int? ExitDate { get; set; }
        public double? ExitPrice { get; set; }
        public double? PL { get; set; }
    }
}