using System;

namespace Screen3.Entity
{
    public class IndHeikinEntity
    {
        public String T { get; set; } // Code, Ticker
        public int P { get; set; } // Period

#nullable enable
        public double? High { get; set; }
        public double? Open { get; set; }
        public double? Close { get; set; }
        public double? Low { get; set; }

#nullable disable
    }
}