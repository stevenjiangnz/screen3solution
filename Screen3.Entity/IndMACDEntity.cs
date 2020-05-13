using System;

namespace Screen3.Entity
{
    public class IndMACDEntity
    {
        public String T { get; set; } // Code, Ticker
        public int P { get; set; } // Period

#nullable enable
        public double? MACD { get; set; }
        public double? Hist { get; set; }
        public double? Signal { get; set; }

#nullable disable
    }
}
