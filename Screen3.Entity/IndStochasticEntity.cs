using System;

namespace Screen3.Entity
{
    public class IndStochasticEntity
    {
        public String T { get; set; } // Code, Ticker
        public int P { get; set; } // Period

#nullable enable
        public double? K { get; set; }
        public double? D { get; set; }

#nullable disable
    }
}